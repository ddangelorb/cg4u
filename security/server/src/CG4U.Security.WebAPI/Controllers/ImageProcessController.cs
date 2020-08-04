using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Services.Controllers;
using CG4U.Core.Services.Interfaces;
using CG4U.Core.Services.ViewModels;
using CG4U.Security.Domain.Configuration;
using CG4U.Security.Domain.Configuration.Repository;
using CG4U.Security.Domain.ImageProcess;
using CG4U.Security.Domain.ImageProcess.Commands;
using CG4U.Security.Domain.ImageProcess.Models;
using CG4U.Security.Domain.ImageProcess.Repository;
using CG4U.Security.Domain.Persons.Repository;
using CG4U.Security.Services.Interfaces;
using CG4U.Security.WebAPI.Hubs;
using CG4U.Security.WebAPI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin,UserSecurity")]
    public class ImageProcessController : BaseController
    {
        private readonly IImageProcessRepository _imageProcessRepository;
        private readonly IVideoCameraRepository _videoCameraRepository;
        private readonly IPersonRepository _personRepository;
        private IImageProcessAdapter _imageProcessAdapter;
        private IHubContext<AlertHub> _context;

        public ImageProcessController(IMediatorHandler mediator,
                                      IImageProcessRepository imageProcessRepository,
                                      IVideoCameraRepository videoCameraRepository,
                                      IPersonRepository personRepository,
                                      IImageProcessAdapter imageProcessAdapter,
                                      IHubContext<AlertHub> context,
                                      UserManager<IdentityUser> userManager,
                                      IUserAdapter userAdapter,
                                      INotificationHandler<DomainNotification> notifications,
                                      IStringLocalizer<ImageProcessController> localizer,
                                      ILogger<ImageProcessController> logger)
            : base(notifications, userManager, userAdapter, mediator, localizer, logger)
        {
            _imageProcessRepository = imageProcessRepository;
            _videoCameraRepository = videoCameraRepository;
            _personRepository = personRepository;
            _imageProcessAdapter = imageProcessAdapter;
            _context = context;
        }

        [HttpGet("{returnResponse}")]
        //[Authorize(Policy = "ImageProcess.ListByReturnResponseAnalyzesAsync")]
        public async Task<IEnumerable<ImageProcessViewModel>> ListByReturnResponseQueryAsync(string returnResponseQuery)
        {
            var imageProcesses = await _imageProcessRepository.ListByReturnResponseQueryAsync(returnResponseQuery);
            return Mapper.Map<IEnumerable<ImageProcess>, IEnumerable<ImageProcessViewModel>>(imageProcesses);
        }

        [HttpPost]
        //[Authorize(Policy = "ImageProcess.AddByVideoCamera")]
        public async Task<IActionResult> AddByVideoCameraAsync([FromBody]ImageProcessViewModel viewModel)
        {
            if (!IsModelStateValid()) return Response();

            var videoCamera = await _videoCameraRepository.GetByIdAsync(viewModel.IdVideoCameras);
            if (videoCamera == null)
            {
                NotifyError("", "VideoCamera not found");
                return Response();
            }

            var userLoggedInDB = await GetUserDbByIdentityAsync(HttpContext);
            if (userLoggedInDB == null) return Response();
            var userLoggedIn = Mapper.Map<UserViewModel, User>(userLoggedInDB);

            var imageProcess = await AddAsync(viewModel, userLoggedIn);
            if (imageProcess == null)
            {
                NotifyError("", "ImageProcess not inserted or found");
                return Response();
            }

            foreach (var analyzeRequestVideoCamera in videoCamera.AnalyzesRequests)
            {
                var imageProcessAnalyze = await AddAnalyzeAsync(userLoggedIn, imageProcess, analyzeRequestVideoCamera);
                if (imageProcessAnalyze == null)
                {
                    NotifyError("", "ImageProcessAnalyze not inserted or found");
                    return Response();
                }

                var adapterReturnResponse = await _imageProcessAdapter.AnalyzeImageProcessAsync(videoCamera, imageProcess, analyzeRequestVideoCamera);
                if (adapterReturnResponse == null)
                    NotifyError("", "ImageProcessAdapter.AnalyzeImage Error");

                await UpdateAnalyzeAsync(userLoggedIn, imageProcessAnalyze, adapterReturnResponse);
            }

            return Response();
        }

        private async Task<ImageProcess> AddAsync(ImageProcessViewModel viewModel, User userLoggedIn)
        {
            var idReference = Guid.NewGuid().ToString();
            var domainModel = Mapper.Map<ImageProcessModel>(viewModel);

            domainModel.IpUserRequest = GetClientIPAddress();
            domainModel.IdReference = new Guid(idReference);
            var cmd = new AddImageProcessCommand(userLoggedIn, domainModel);
            await _mediator.SendCommand(cmd);
                                
            return await _imageProcessRepository.GetByIdReferenceAsync(idReference);
        }

        private async Task<ImageProcessAnalyze> AddAnalyzeAsync(User userLoggedIn, ImageProcess imageProcess, AnalyzeRequest analyzeRequest)
        {
            var idReference = Guid.NewGuid().ToString();
            var domainModelAnalyze = new ImageProcessAnalyzeModel
            {
                IdReference = new Guid(idReference),
                IdImageProcesses = imageProcess.Id,
                IdAnalyzesRequestsVideoCameras = analyzeRequest.IdAnalyzesRequestsVideoCameras,
                DtAnalyze = DateTime.Now,
                ReturnResponseType = "JSON",
                Commited = 0,
                Active = 1
            };

            var cmdAddAnalyze = new AddImageProcessAnalyzeCommand(userLoggedIn, domainModelAnalyze);
            await _mediator.SendCommand(cmdAddAnalyze);

            return await _imageProcessRepository.GetAnalyzeByIdReferenceAsync(idReference);
        }

        private async Task UpdateAnalyzeAsync(User userLoggedIn, ImageProcessAnalyze imageProcessAnalyze, string returnResponse)
        {
            var domainModelAnalyze = Mapper.Map<ImageProcessAnalyze, ImageProcessAnalyzeModel>(imageProcessAnalyze);
            domainModelAnalyze.Commited = 1;
            domainModelAnalyze.ReturnResponse = returnResponse;

            var cmdUpdateAnalyze = new UpdateImageProcessAnalyzeCommand(userLoggedIn, domainModelAnalyze);
            await _mediator.SendCommand(cmdUpdateAnalyze);

            var alerts = await _imageProcessAdapter.ListAlertsByUserAndResponseAsync(userLoggedIn.Id, returnResponse);
            if (alerts != null)
            {
                var messageHub = new AlertHub(_context);
                foreach (var alert in alerts)
                    await messageHub.SendAlert(alert);
            }
        }
    }
}
