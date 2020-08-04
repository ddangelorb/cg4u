using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using CG4U.Core.Services.Controllers;
using CG4U.Core.Services.Services;
using CG4U.Core.Services.ViewModels;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using Microsoft.Extensions.Options;
using CG4U.Core.Services.Interfaces;

namespace CG4U.Auth.WebAPI.Controllers
{
    [Route("[controller]/[action]")]    
    public class AccountController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private MessageSender _messageSender;

        public AccountController(
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator,
            IOptions<EmailSender> emailSenderOption,
            UserManager<IdentityUser> userManager,
            IUserAdapter userAdapter,
            IStringLocalizer<AccountController> localizer,
            ILogger<AccountController> logger,
            SignInManager<IdentityUser> signInManager
            )
            : base(notifications, userManager, userAdapter, mediator, localizer, logger)
        {
            _signInManager = signInManager;
            _messageSender = new MessageSender(
                emailSenderOption.Value.Email,
                emailSenderOption.Value.Password,
                emailSenderOption.Value.Host,
                int.Parse(emailSenderOption.Value.Port)
            );
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> RegisterAsync([FromBody] UserViewModel model)
        {
            if (!IsModelStateValid()) return Response();

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                foreach (var role in model.Roles)
                {
                    //await _userManager.AddClaimAsync(user, new Claim("Eventos", "Gravar"));
                    var resultRole = await _userManager.AddToRoleAsync(user, role.ToString());
                    if (!resultRole.Succeeded)
                    {
                        AddIdentityErrors(resultRole);
                        return Response(model);
                    }
                }

                model.IdUserIdentity = user.Id;
                var userAdded = await _userAdapter.AddDbAsync(user, model);
                if (!userAdded)
                {
                    NotifyError(_userAdapter.GetErrors());
                    return Response();
                }

                var tokenConfirmation = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(
                    "ConfirmEmailAsync",
                    "Account",
                    new
                    {
                        id = user.Id,
                        idSystems = model.IdSystems,
                        token = tokenConfirmation
                    },
                    protocol: Request.Scheme);
                
                await _messageSender.SendEmailAsync(
                    user.Email,
                    "CG4U: Confirm your account",
                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                _logger.LogInformation(1, string.Format("Login Created Succesfully done for Email:{0}", model.Email));
                return Response();
            }

            AddIdentityErrors(result);
            return Response(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> LoginAsync([FromBody] UserViewModel model)
        {
            if (!IsModelStateValid()) return Response();

            var user = await GetUserLoggedInAsync(model.Email, model.Password);
            if (user == null) return Response(model);

            var userModel = await _userAdapter.GetDbByIdentityAsync(user);
            if (userModel == null)
            {
                NotifyError("", "Login.UnableToGetLoginDB");
                return Response(model);
            }
            if (! await _userAdapter.IsUserHaveAccessSystem(user, model.IdSystems))
            {
                NotifyError("", "Login.UserDoesntHaveAccessSystem");
                return Response(model);
            }

            userModel.IdSystems = model.IdSystems;
            userModel.IdLanguages = model.IdLanguages;
            var jwtToken = await _userAdapter.GenerateJwtTokenAsync(user, userModel);
            if (jwtToken != null)
            {
                _logger.LogInformation(1, string.Format("Login Succesfully done for Email:{0}", model.Email));
                return jwtToken;
            }

            NotifyError(_userAdapter.GetErrors());
            NotifyError("", "Login.UnableToLogin");
            return Response(model);
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync(string id, int idSystems, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                NotifyError(string.Empty, "Login.InvalidUserOrToken");
                return Response();
            }

            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!emailConfirmationResult.Succeeded)
            {
                foreach(var error in emailConfirmationResult.Errors)
                {
                    NotifyError(error.Code, error.Description);
                    return Response();
                }
            }

            var userAdded = await _userAdapter.EnableDbAsync(user, idSystems);
            if (!userAdded)
            {
                NotifyError(_userAdapter.GetErrors());
                return Response();
            }

            _logger.LogInformation(1, string.Format("Email confirmed, you can now log in for:{0}", user.Email));
            return Response();
        } 

        [HttpPost]
        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                NotifyError(string.Empty, "Login.InvalidUserOrToken");
                return Response();
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetUrl = Url.Action("ResetPasswordAsync", "Account", new { id = user.Id, token = passwordResetToken }, Request.Scheme);

            await _messageSender.SendEmailAsync(
                email, 
                "CG4U: Password reset", 
                $"Click <a href=\"" + passwordResetUrl + "\">here</a> to reset your password")
                ;

            _logger.LogInformation(1, string.Format("Check your email for a password reset link:{0}", user.Email));
            return Response();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(string id, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                NotifyError(string.Empty, "Login.InvalidUserOrToken");
                return Response();
            }

            var resetPwdResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!resetPwdResult.Succeeded)
            {
                NotifyError(string.Empty, "Login.CannotSetNewPassword");
                return Response();
            }

            _logger.LogInformation(1, string.Format("New Password setted for:{0}", user.Email));
            return Response();
        } 

        private void AddIdentityErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotifyError(result.ToString(), error.Description);
            }
        }

        private async Task<IdentityUser> GetUserLoggedInAsync(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    NotifyError(string.Empty, "Login.MustConfirmEmail");
                    return null;
                }
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            if (!result.Succeeded) 
            {
                NotifyError(string.Empty, "Login.InvalidLogin");
                return null;
            }

            return user;
        }
    }
}
