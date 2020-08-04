using System;
using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Services.Controllers;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.ViewModels;
using CG4U.Security.Domain.Persons;
using CG4U.Security.Domain.Persons.Commands;
using CG4U.Security.Domain.Persons.Models;
using CG4U.Security.Domain.Persons.Repository;
using CG4U.Security.Services.Interfaces;
using CG4U.Security.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin,UserSecurityAdmin")]
    public class PersonController : BaseController
    {
        private readonly IPersonRepository _personRepository;
        private IPersonAdapter _personAdapter;

        public PersonController(IMediatorHandler mediator,
                                IPersonRepository personRepository,
                                IPersonAdapter personAdapter,
                                UserManager<IdentityUser> userManager,
                                IUserAdapter userAdapter,
                                INotificationHandler<DomainNotification> notifications,
                                IStringLocalizer<ConfigurationController> localizer,
                                ILogger<ConfigurationController> logger)
            : base(notifications, userManager, userAdapter, mediator, localizer, logger)
        {
            _personRepository = personRepository;
            _personAdapter = personAdapter;
        }

        [HttpGet("{id:int}")]
        //[Authorize(Policy = "Person.GetById")]
        public async Task<PersonViewModel> GetByIdAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            return Mapper.Map<Person, PersonViewModel>(person);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Policy = "Person.GetPersonGroupById")]
        public async Task<PersonGroupViewModel> GetPersonGroupByIdAsync(int id)
        {
            var personGroup = await _personRepository.GetPersonGroupByIdAsync(id);
            return Mapper.Map<PersonGroup, PersonGroupViewModel>(personGroup);
        }

        [HttpPost]
        //[Authorize(Policy = "Person.Add")]
        public async Task<IActionResult> AddPersonGroupAsync([FromBody]PersonGroupViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<PersonGroupModel>(viewModel);

            var cmd = new AddPersonGroupCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            var personGroup = await _personRepository.GetPersonGroupByIdApiAsync(viewModel.IdApi);
            if (personGroup == null)
            {
                NotifyError("", "Could not get PersonGroup by IdApi");
                return Response();
            }

            if (!await _personAdapter.CreatePersonGroupAsync(personGroup))
            {
                NotifyError("", "Could not create PersonGroup on Adapter");
                return Response();
            }

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Person.Add")]
        public async Task<IActionResult> AddPersonAsync([FromBody]PersonViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var domain = Mapper.Map<Person>(viewModel);
            var personIdApi = await _personAdapter.CreatePersonAsync(domain);
            if (personIdApi == null)
            {
                NotifyError("", "Could not create Person on Adapter");
                return Response();
            }

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<PersonModel>(viewModel);
            domainModel.IdApi = new Guid(personIdApi);

            var cmd = new AddPersonCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Person.Add")]
        public async Task<IActionResult> AddPersonFaceAsync([FromBody]FaceViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var person = await _personRepository.GetByIdAsync(viewModel.IdPersons);
            if (person == null)
            {
                NotifyError("", "Person not found");
                return Response();
            }

            var faceId = await _personAdapter.AddPersonFaceAsync(person, viewModel.Image);
            if (faceId == null)
            {
                NotifyError("", "Could not add Person Face on Adapter");
                return Response();
            }

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<FaceModel>(viewModel);
            domainModel.FaceId = new Guid(faceId);
            var cmd = new AddPersonFaceCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Person.Add")]
        public async Task<IActionResult> TrainPersonGroupAsync([FromBody]PersonGroupViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var personGroup = await _personRepository.GetPersonGroupByIdApiAsync(viewModel.IdApi);
            if (personGroup == null)
            {
                NotifyError("", "Could not get PersonGroup by IdApi");
                return Response();
            }

            var trainSent = await _personAdapter.TrainPersonGroupAsync(personGroup);
            if (!trainSent)
            {
                NotifyError("", "Could not train Person Group on Adapter");
                return Response();
            }

            return Response();
        }

        [HttpGet("{idPersonGroupApi}")]
        //[Authorize(Policy = "Donation.GetById")]
        public async Task<Status> GetTrainPersonGroupAsync(string idPersonGroupApi)
        {
            var personGroup = await _personRepository.GetPersonGroupByIdApiAsync(new Guid(idPersonGroupApi));
            if (personGroup == null)
            {
                _logger.LogError("GetTrainPersonGroupAsync::Could not get PersonGroup by IdApi");
                return Status.Error;
            }

            return await _personAdapter.GetTrainingStatusPersonGroupAsync(personGroup);
        }
    }
}
