using System;
using System.Globalization;
using System.Linq;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Services.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using CG4U.Core.Services.Interfaces;
using AutoMapper;
using CG4U.Core.Common.Domain.Models;
using System.Net;

namespace CG4U.Core.Services.Controllers
{
    public abstract class BaseController : Controller
    {
        //private static string[] _supportedLanguages = { "pt", "pt-BR", "en", "en-GB", "en-US", "es", "fr", "it", "de" };
        private static string[] _supportedLanguages = { "pt", "pt-BR", "en", "en-GB", "en-US", "es" };
        private readonly DomainNotificationHandler _notifications;

        protected UserManager<IdentityUser> _userManager;
        protected IUserAdapter _userAdapter;
        protected IMediatorHandler _mediator;
        protected IStringLocalizer _localizer;
        protected ILogger _logger;

        public BaseController(INotificationHandler<DomainNotification> notifications,
                              UserManager<IdentityUser> userManager,
                              IUserAdapter userAdapter,
                              IMediatorHandler mediator,
                              IStringLocalizer localizer,
                              ILogger logger)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _userManager = userManager;
            _userAdapter = userAdapter;
            _mediator = mediator;
            _localizer = localizer;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string fromUrl, string toUrl)
        {
            if (!_supportedLanguages.Contains(culture))
            {
                NotifyError(string.Empty, "Unsupported language setting.");
                return LocalRedirect(fromUrl);
            }

            _localizer = (IStringLocalizer<BaseController>)_localizer.WithCulture(new CultureInfo(culture));
            return LocalRedirect(toUrl);
        }

        protected string GetClientIPAddress()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ip != null) return ip;

            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            ip = heserver.AddressList[0].ToString();
            if (ip != null) return ip;

            return string.Empty;
        }

        protected async Task<UsersRequest> GetUsersRequestAsync(HttpContext context, params string[] listIdUserIdentity)
        {
            var userLoggedInDB = await GetUserDbByIdentityAsync(context);
            if (userLoggedInDB == null) 
            {
                NotifyError(string.Empty, "User Logged In not found.");
                return null;
            }
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var listUsersViewModel = new List<User>();
            foreach (var idUserIdentity in listIdUserIdentity)
            {
                var userViewModelDB = await GetUserDbByIdentityAsync(idUserIdentity);
                if (userViewModelDB == null)
                {
                    NotifyError(string.Empty, "User DataBase/ViewModel not found.");
                    return null;
                }
                listUsersViewModel.Add(Mapper.Map<UserViewModel, User>(userViewModelDB));
            }

            return new UsersRequest()
            {
                UserLoggedIn = userLoggedIn,
                ListUsersViewModel = listUsersViewModel
            };
        }

        protected virtual async Task<UserViewModel> GetUserDbByIdentityAsync(string id)
        {
            var identityUser = await GetIdentityUserAsync(id);
            if (identityUser == null)
            {
                NotifyError(string.Empty, "User Identity not found.");
                return null;
            }

            return await GetUserDbByIdentity(identityUser);
        }

        protected virtual async Task<UserViewModel> GetUserDbByIdentityAsync(HttpContext context)
        {
            var identityUser = await GetIdentityUserAsync(context);
            if (identityUser == null)
            {
                NotifyError(string.Empty, "User Identity not found.");
                return null;
            }

            return await GetUserDbByIdentity(identityUser);
        }

        protected bool ValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = "OK" 
                            //data = result -> 'Operation is not supported on this platform
                            //https://github.com/JamesNK/Newtonsoft.Json/issues/1404
                });
            }

            var errorsNotifications = _notifications.GetNotifications().Select(n => _localizer[n.Value]);
            LogNotificationErrors(errorsNotifications);
            return BadRequest(new
            {
                success = false,
                errors = errorsNotifications
            });
        }

        protected void LogNotificationErrors(IEnumerable<LocalizedString> errors)
        {
            foreach (var error in errors)
            {
                _logger.LogError(_localizer[error]);
            }
        }

        protected void NotifyError(ICollection<string> errors)
        {
            foreach (var error in errors)
            {
                NotifyError(string.Empty, error);
            }
        }

        protected void NotifyError(string key, string value)
        {
            NotifyError(null, key, value);
        }

        protected void NotifyError(Exception ex, string key, string value)
        {
            var valueLocalized = _localizer[value];

            _logger.LogError(ex, string.Concat(key, "::", valueLocalized));
            _mediator.PublishEvent(new DomainNotification(_logger, key, valueLocalized));
        }

        protected bool IsModelStateValid()
        {
            if (ModelState.IsValid) return true;

            NotifyViewModelInvalid();
            return false;
        }

        protected async Task<IdentityUser> GetIdentityUserAsync(HttpContext context)
        {
            if (context == null 
                || context.User == null
                || context.User.Claims == null
                || context.User.Claims.Count() == 0)
            {
                NotifyError(string.Empty, "Invalid User Context.");
                return null;
            }

            var userId = context.User.Claims.First((arg) => arg.Type.Equals("sub")).Value;
            if (userId == null)
            {
                NotifyError(string.Empty, "Invalid User.Id Context.");
                return null;
            }

            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
            {
                NotifyError("", "User Identity not found.");
                return null;
            }

            return identityUser;
        }

        private async Task<IdentityUser> GetIdentityUserAsync(string id)
        {
            var identityUser = await _userManager.FindByIdAsync(id);
            if (identityUser == null)
            {
                NotifyError("", "User Identity not found.");
                return null;
            }

            return identityUser;
        }

        private async Task<UserViewModel> GetUserDbByIdentity(IdentityUser identityUser)
        {
            var userDB = await _userAdapter.GetDbByIdentityAsync(identityUser);
            if (userDB == null)
            {
                NotifyError(string.Empty, "User not found.");
                return null;
            }
            userDB.IdentityUser = identityUser;

            var identityUserRoles = await _userManager.GetRolesAsync(userDB.IdentityUser);
            if (identityUserRoles == null || identityUserRoles.Count() == 0)
            {
                NotifyError(string.Empty, "User not found.");
                return null;
            }
            foreach(var userRole in identityUserRoles)
            {
                userDB.Roles.Add((UserRoles)Enum.Parse(typeof(UserRoles), userRole));
            }

            return userDB;
        }

        private void NotifyViewModelInvalid()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                var errorMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(string.Empty, errorMsg);
            }
        }
    }
}
