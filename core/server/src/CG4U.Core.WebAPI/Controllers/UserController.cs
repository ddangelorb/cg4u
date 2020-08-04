using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Domain.Users.Commands;
using CG4U.Core.Domain.Users.Models;
using CG4U.Core.Domain.Users.Repository;
using CG4U.Core.Services.Authorization;
using CG4U.Core.Services.Controllers;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.ViewModels;
using CG4U.Core.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CG4U.Core.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    //TODO: Add a new profile less admin to do that
    [Authorize(Roles = "Admin,UserDonate")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IMediatorHandler mediator,
                              IUserRepository userRepository,
                              UserManager<IdentityUser> userManager,
                              IUserAdapter userAdapter,
                              INotificationHandler<DomainNotification> notifications,
                              IStringLocalizer<UserController> localizer,
                              ILogger<UserController> logger)
            : base(notifications, userManager, userAdapter, mediator, localizer, logger)
        {
            _userRepository = userRepository;
        }

        //GetByIdUserIdentity
        [HttpGet("{idUserIdentity}")]
        //[Authorize(Policy = "Donation.GetById")]
        //TODO: Add a new profile less admin to do that
        public async Task<UserViewModel> GetByIdUserIdentityAsync(string idUserIdentity)
        {
            var user = await _userRepository.GetByIdUserIdentityAsync(idUserIdentity);
            return Mapper.Map<User, UserViewModel>(user);
        }

        [HttpGet("{idUserIdentity}/{idSystems:int}")]
        //[Authorize(Policy = "Donation.GetById")]
        //TODO: Add a new profile less admin to do that
        public async Task<bool> IsUserHasAccessSystem(string idUserIdentity, int idSystems)
        {
            return await _userRepository.IsUserHasAccessSystem(idUserIdentity, idSystems);
        }

        [HttpPost]
        //[Authorize(Policy = "Desired.Add")]
        //TODO: Add a new profile less admin to do that
        public async Task<IActionResult> AddUserAsync([FromBody]UserViewModel userViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedIn = Mapper.Map<UserViewModel, User>(userViewModel);

            var cmd = new AddUserCommand(
                userLoggedIn,
                Mapper.Map<UserModel>(userViewModel));
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        //Enable
        [HttpPut]
        //[Authorize(Policy = "Desired.Add")]
        //TODO: Add a new profile less admin to do that
        public async Task<IActionResult> EnableUserAsync([FromBody]UserSystemViewModel userSystemViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();

            var cmd = new EnableUserCommand(
                Mapper.Map<UserViewModel, User>(userLoggedInDB),
                userSystemViewModel.IdUserIdentity,
                userSystemViewModel.IdSystems 
            );
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        //Disable
        [HttpPut("{idUserIdentity:int}")]
        //[Authorize(Policy = "Desired.Add")]
        //TODO: Add a new profile less admin to do that
        public async Task<IActionResult> DisableUserAsync(int idUser)
        {
            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();

            var cmd = new DisableUserCommand(
                Mapper.Map<UserViewModel, User>(userLoggedInDB), 
                idUser);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }


        [HttpPut]
        //[Authorize(Policy = "Desired.Add")]
        //TODO: Add a new profile less admin to do that
        public async Task<IActionResult> UpdateUserAsync([FromBody]UserViewModel userViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();

            var cmd = new UpdateUserCommand(
                Mapper.Map<UserViewModel, User>(userLoggedInDB), 
                Mapper.Map<UserModel>(userViewModel));
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        //AddSystem
        [HttpPost]
        //[Authorize(Policy = "Desired.Add")]
        //TODO: Add a new profile less admin to do that
        public async Task<IActionResult> AddSystemAsync([FromBody]UserSystemViewModel userSystemViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();

            var cmd = new AddSystemUserCommand(
                Mapper.Map<UserViewModel, User>(userLoggedInDB),
                userSystemViewModel.IdUserIdentity,
                userSystemViewModel.IdSystems);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        protected override async Task<UserViewModel> GetUserDbByIdentityAsync(HttpContext context)
        {
            var identityUser = await GetIdentityUserAsync(context);
            if (identityUser == null) return null;

            var user = await GetByIdUserIdentityAsync(identityUser.Id);
            user.IdentityUser = identityUser;

            return user;
        }
    }
}
