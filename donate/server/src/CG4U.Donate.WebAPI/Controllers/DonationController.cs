using System.Collections.Generic;
using AutoMapper;
using CG4U.Core.Services.Controllers;
using CG4U.Donate.Domain.Donations.Commands;
using CG4U.Donate.Domain.Donations.Models;
using CG4U.Donate.Domain.Donations.Repository;
using CG4U.Donate.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;
using CG4U.Donate.Domain.Donations;
using CG4U.Core.Services.Interfaces;

namespace CG4U.Donate.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin,UserDonate")]
    public class DonationController : BaseController
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IDesiredRepository _desiredRepository;
        private readonly IGivenRepository _givenRepository;

        public DonationController(IMediatorHandler mediator,
                                  IDonationRepository donationRepository,
                                  IDesiredRepository desiredRepository,
                                  IGivenRepository givenRepository,
                                  UserManager<IdentityUser> userManager,
                                  IUserAdapter userAdapter,
                                  INotificationHandler<DomainNotification> notifications,
                                  IStringLocalizer<DonationController> localizer,
                                  ILogger<DonationController> logger)
            : base(notifications, userManager, userAdapter, mediator, localizer, logger)
        {
            _donationRepository = donationRepository;
            _desiredRepository = desiredRepository;
            _givenRepository = givenRepository;
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Donation.GetById")]
        public async Task<DonationViewModel> GetDonationByIdSystemLanguageAsync(int id, int idSystems, int idLanguages)
        {
            var donation = await _donationRepository.GetByIdSystemLanguageAsync(id, idSystems, idLanguages);
            return Mapper.Map<DonationViewModel>(donation);
        }

        [HttpGet("{name}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Donation.GetDonationsByLanguageAndName")]
        public async Task<IEnumerable<DonationViewModel>> ListDonationsByLanguageAndNameAsync(string name, int idSystems, int idLanguages)
        {
            var donations = await _donationRepository.GetByLanguageAndNameAsync(idSystems, idLanguages, name);
            return Mapper.Map<IEnumerable<DonationViewModel>>(donations);
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Desired.GetById")]
        public async Task<DesiredViewModel> GetDesiredByIdSystemLanguageAsync(int id, int idSystems, int idLanguages)
        {
            var desired = await _desiredRepository.GetByIdSystemLanguageAsync(id, idSystems, idLanguages);
            return Mapper.Map<Desired, DesiredViewModel>(desired);
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Desired.ListByOwnerAsync")]
        public async Task<IEnumerable<DesiredViewModel>> ListDesiredsByOwnerAsync(int id, int idSystems, int idLanguages)
        {
            var desireds = await _desiredRepository.ListByOwnerAsync(id, idSystems, idLanguages);
            return Mapper.Map<IEnumerable<DesiredViewModel>>(desireds);
        }

        [HttpPost]
        //[Authorize(Policy = "Desired.Add")]
        public async Task<IActionResult> AddDesiredAsync([FromBody]DesiredViewModel desiredViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var usersRequest = await GetUsersRequestAsync(HttpContext, desiredViewModel.User.IdUserIdentity);
            if (usersRequest == null) return Response();

            var domainModel = Mapper.Map<DesiredViewModel, DesiredModel>(desiredViewModel);
            domainModel.User = usersRequest.ListUsersViewModel[0];

            var cmd = new AddDesiredCommand(usersRequest.UserLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Desired.Update")]
        public async Task<IActionResult> UpdateDesiredAsync([FromBody]DesiredViewModel desiredViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var usersRequest = await GetUsersRequestAsync(HttpContext, desiredViewModel.User.IdUserIdentity);
            if (usersRequest == null) return Response();

            var domainModel = Mapper.Map<DesiredViewModel, DesiredModel>(desiredViewModel);
            domainModel.User = usersRequest.ListUsersViewModel[0];

            var cmd = new UpdateDesiredCommand(usersRequest.UserLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Route("Donation/DisableDesiredAsync/{id:int}")]
        //[Authorize(Policy = "Desired.Disable")]
        public async Task<IActionResult> DisableDesiredAsync([FromBody] int id)
        {
            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var cmd = new DisableDesiredCommand(userLoggedIn, id);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Given.GetById")]
        public async Task<GivenViewModel> GetGivenByIdSystemLanguageAsync(int id, int idSystems, int idLanguages)
        {
            var given = await _givenRepository.GetByIdSystemLanguageAsync(id, idSystems, idLanguages);
            return Mapper.Map<Given, GivenViewModel>(given);
        }

        [HttpGet("{id:int}/{idSystems:int}/{idLanguages:int}")]
        //[Authorize(Policy = "Given.ListByOwnerAsync")]
        public async Task<IEnumerable<GivenViewModel>> ListGivensByOwnerAsync(int id, int idSystems, int idLanguages)
        {
            var givens = await _givenRepository.ListByOwnerAsync(id, idSystems, idLanguages);
            return Mapper.Map<IEnumerable<GivenViewModel>>(givens);
        }

        [HttpPost]
        //[Authorize(Policy = "Given.Add")]
        public async Task<IActionResult> AddGivenAsync([FromBody]GivenViewModel givenViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var usersRequest = await GetUsersRequestAsync(HttpContext, givenViewModel.User.IdUserIdentity);
            if (usersRequest == null) return Response();

            var domainModel = Mapper.Map<GivenViewModel, GivenModel>(givenViewModel);
            domainModel.User = usersRequest.ListUsersViewModel[0];

            var cmd = new AddGivenCommand(usersRequest.UserLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Given.Update")]
        public async Task<IActionResult> UpdateGivenAsync([FromBody]GivenViewModel givenViewModel)
        {
            if (!IsModelStateValid()) return Response();

            var usersRequest = await GetUsersRequestAsync(HttpContext, givenViewModel.User.IdUserIdentity);
            if (usersRequest == null) return Response();

            var domainModel = Mapper.Map<GivenViewModel, GivenModel>(givenViewModel);
            domainModel.User = usersRequest.ListUsersViewModel[0];

            var cmd = new UpdateGivenCommand(usersRequest.UserLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Given.AddGivenImage")]
        public async Task<IActionResult> AddGivenImageAsync([FromBody]GivenViewModel givenViewModel)
        {
            if (!IsModelStateValid()) return Response();

            if (givenViewModel.Img == null || givenViewModel.Img.Length == 0)
            {
                NotifyError(string.Empty, "Given image not found.");
                return Response();
            }

            var usersRequest = await GetUsersRequestAsync(HttpContext, givenViewModel.User.IdUserIdentity);
            if (usersRequest == null) return Response();

            var domainModel = Mapper.Map<GivenViewModel, GivenModel>(givenViewModel);
            domainModel.User = usersRequest.ListUsersViewModel[0];

            var cmd = new AddGivenImageCommand(usersRequest.UserLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Given.Disable")]
        public async Task<IActionResult> DisableGivenAsync([FromBody] int id)
        {
            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var cmd = new DisableGivenCommand(userLoggedIn, id);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }
    }
}
