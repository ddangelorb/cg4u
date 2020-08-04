using System;
using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Security.Domain.Configuration.Events;
using CG4U.Security.Domain.Configuration.Models;
using CG4U.Security.Domain.Configuration.Repository;
using CG4U.Security.Domain.Persons.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Configuration.Commands
{
    public class ConfigurationCommandHandler : CommandHandler,
        IAsyncNotificationHandler<AddVideoCameraCommand>,
        IAsyncNotificationHandler<AddAnalyzeRequestCommand>,
        IAsyncNotificationHandler<AddAlertCommand>,
        IAsyncNotificationHandler<AddAlertConnectionPersonGroupCommand>,
        IAsyncNotificationHandler<AddAnalyzeRequestConnectionVideoCameraCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IAlertRepository _alertRepository;
        private readonly IAnalyzeRequestRepository _analyzeRequestRepository;
        private readonly IVideoCameraRepository _videoCameraRepository;
        private readonly IPersonRepository _personRepository;

        public ConfigurationCommandHandler( IMediatorHandler mediator,
                                            IAlertRepository alertRepository,
                                            IAnalyzeRequestRepository analyzeRequestRepository,
                                            IVideoCameraRepository videoCameraRepository,
                                            IPersonRepository personRepository,
                                            ILogger<ConfigurationCommandHandler> logger,
                                            INotificationHandler<DomainNotification> notifications)
            : base(mediator, (DomainNotificationHandler)notifications, logger)
        {
            _mediator = mediator;
            _alertRepository = alertRepository;
            _analyzeRequestRepository = analyzeRequestRepository;
            _videoCameraRepository = videoCameraRepository;
            _personRepository = personRepository;
        }

        public async Task Handle(AddVideoCameraCommand notification)
        {
            var entityCurrent = Mapper.Map<VideoCameraModel, VideoCamera>(notification.VideoCameraModel);

            if (!IsEntityValid(entityCurrent)) return;

            var personGroup = await _personRepository.GetPersonGroupByIdAsync(notification.VideoCameraModel.IdPersonGroups);
            if (personGroup == null)
            {
                NotifyErrorValidations("AddVideoCameraCommand", "PersonGroup not found");
                return;
            }

            await _videoCameraRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new VideoCameraAddedEvent(_logger, Mapper.Map<VideoCameraModel>(notification.VideoCameraModel)));
        }

        public async Task Handle(AddAnalyzeRequestCommand notification)
        {
            var entityCurrent = Mapper.Map<AnalyzeRequestModel, AnalyzeRequest>(notification.AnalyzeRequestModel);

            if (!IsEntityValid(entityCurrent)) return;

            await _analyzeRequestRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new AnalyzeRequestAddedEvent(_logger, Mapper.Map<AnalyzeRequestModel>(notification.AnalyzeRequestModel)));
        }

        public async Task Handle(AddAlertCommand notification)
        {
            var entityCurrent = Mapper.Map<AlertModel, Alert>(notification.AlertModel);

            if (!IsEntityValid(entityCurrent)) return;

            await _alertRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new AlertAddedEvent(_logger, Mapper.Map<AlertModel>(notification.AlertModel)));
        }

        public async Task Handle(AddAlertConnectionPersonGroupCommand notification)
        {
            var alert = await _alertRepository.GetByIdAsync(notification.IdAlerts);
            if (alert == null)
            {
                NotifyErrorValidations("AddAlertConnectionPersonGroupCommand", "Alert not found");
                return;
            }

            var personGroup = await _personRepository.GetPersonGroupByIdAsync(notification.IdPersonGroups);
            if (personGroup == null)
            {
                NotifyErrorValidations("AddAlertConnectionPersonGroupCommand", "PersonGroup not found");
                return;
            }

            await _alertRepository.AddConnectionPersonGroupAsync(notification.IdPersonGroups, notification.IdAlerts);

            if (Commit())
                await _mediator.PublishEvent(new AlertConnectionPersonGroupAddedEvent(_logger, notification.IdPersonGroups, notification.IdAlerts));
        }

        public async Task Handle(AddAnalyzeRequestConnectionVideoCameraCommand notification)
        {
            var analyzeRequest = await _analyzeRequestRepository.GetByIdAsync(notification.IdAnalyzesRequests);
            if (analyzeRequest == null)
            {
                NotifyErrorValidations("AddAnalyzeRequestConnectionVideoCameraCommand", "AnalyzeRequest not found");
                return;
            }

            var videoCamera = await _videoCameraRepository.GetByIdAsync(notification.IdVideoCameras);
            if (videoCamera == null)
            {
                NotifyErrorValidations("AddAnalyzeRequestConnectionVideoCameraCommand", "VideoCamera not found");
                return;
            }

            await _analyzeRequestRepository.AddConnectionVideoCameraAsync(notification.IdAnalyzesRequests, notification.IdVideoCameras);

            if (Commit())
                await _mediator.PublishEvent(new AnalyzeRequestConnectionVideoCameraAddedEvent(_logger, notification.IdAnalyzesRequests, notification.IdVideoCameras));
        }
    }
}
