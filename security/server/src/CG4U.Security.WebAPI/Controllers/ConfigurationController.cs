using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Services.Controllers;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.ViewModels;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.Configuration.Commands;
using CG4U.Security.Domain.Configuration.Models;
using CG4U.Security.Domain.Configuration.Repository;
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
    [Authorize(Roles = "Admin")]
    public class ConfigurationController : BaseController
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IAnalyzeRequestRepository _analyzeRequestRepository;
        private readonly IVideoCameraRepository _videoCameraRepository;
        private readonly IPersonRepository _personRepository;

        public ConfigurationController(IMediatorHandler mediator,
                                       IAlertRepository alertRepository,
                                       IAnalyzeRequestRepository analyzeRequestRepository,
                                       IVideoCameraRepository videoCameraRepository,
                                       IPersonRepository personRepository,
                                       UserManager<IdentityUser> userManager,
                                       IUserAdapter userAdapter,
                                       INotificationHandler<DomainNotification> notifications,
                                       IStringLocalizer<ConfigurationController> localizer,
                                       ILogger<ConfigurationController> logger)
            : base(notifications, userManager, userAdapter, mediator, localizer, logger)
        {
            _alertRepository = alertRepository;
            _analyzeRequestRepository = analyzeRequestRepository;
            _videoCameraRepository = videoCameraRepository;
            _personRepository = personRepository;
        }

        [HttpGet("{id:int}")]
        //[Authorize(Policy = "Trade.GetById")]
        public async Task<AnalyzeRequestViewModel> GetAnalyzeRequestByIdAsync(int id)
        {
            var analyzeRequest = await _analyzeRequestRepository.GetByIdAsync(id);
            return Mapper.Map<AnalyzeRequest, AnalyzeRequestViewModel>(analyzeRequest);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Policy = "Trade.GetById")]
        public async Task<AlertViewModel> GetAlertByIdAsync(int id)
        {
            var alert = await _alertRepository.GetByIdAsync(id);
            return Mapper.Map<Alert, AlertViewModel>(alert);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Policy = "Trade.GetById")]
        public async Task<VideoCameraViewModel> GetVideoCameraByIdAsync(int id)
        {
            var videoCamera = await _videoCameraRepository.GetByIdAsync(id);
            return Mapper.Map<VideoCamera, VideoCameraViewModel>(videoCamera);
        }

        [HttpPost]
        //[Authorize(Policy = "AnalyzeRequest.Add")]
        public async Task<IActionResult> AddAnalyzeRequestAsync([FromBody]AnalyzeRequestViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<AnalyzeRequestModel>(viewModel);

            var cmd = new AddAnalyzeRequestCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "Alert.Add")]
        public async Task<IActionResult> AddAlertAsync([FromBody]AlertViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<AlertModel>(viewModel);

            var cmd = new AddAlertCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "VideoCamera.Add")]
        public async Task<IActionResult> AddVideoCameraAsync([FromBody]VideoCameraViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var domainModel = Mapper.Map<VideoCameraModel>(viewModel);

            var cmd = new AddVideoCameraCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "AlertConnectionPersonGroup.Add")]
        public async Task<IActionResult> AddAlertConnectionPersonGroupAsync([FromBody]AlertPersonGroupViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var alert = await _alertRepository.GetByIdAsync(viewModel.IdAlerts);
            if (alert == null)
            {
                NotifyError("", "Could not get Alert by Id");
                return Response();
            }

            var personGroup = await _personRepository.GetPersonGroupByIdAsync(viewModel.IdPersonGroups);
            if (personGroup == null)
            {
                NotifyError("", "Could not get PersonGroup by Id");
                return Response();
            }

            var cmd = new AddAlertConnectionPersonGroupCommand(userLoggedIn, viewModel.IdPersonGroups, viewModel.IdAlerts);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }

        [HttpPost]
        //[Authorize(Policy = "AnalyzeRequestConnectionVideoCamera.Add")]
        public async Task<IActionResult> AddAnalyzeRequestConnectionVideoCameraAsync([FromBody]AnalyzeRequestVideoCameraViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var analyzeRequest = await _analyzeRequestRepository.GetByIdAsync(viewModel.IdAnalyzesRequests);
            if (analyzeRequest == null)
            {
                NotifyError("", "Could not get AnalyzeRequest by Id");
                return Response();
            }

            var videoCamera = await _videoCameraRepository.GetByIdAsync(viewModel.IdVideoCameras);
            if (videoCamera == null)
            {
                NotifyError("", "Could not get VideoCamera by Id");
                return Response();
            }

            var cmd = new AddAnalyzeRequestConnectionVideoCameraCommand(userLoggedIn, viewModel.IdAnalyzesRequests, viewModel.IdVideoCameras);
            await _mediator.SendCommand(cmd);

            return Response(cmd);
        }
    }
}
