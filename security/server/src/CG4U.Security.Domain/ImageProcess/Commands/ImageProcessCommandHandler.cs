using System;
using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Security.Domain.ImageProcess.Events;
using CG4U.Security.Domain.ImageProcess.Models;
using CG4U.Security.Domain.ImageProcess.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.ImageProcess.Commands
{
    public class ImageProcessCommandHandler : CommandHandler,
        IAsyncNotificationHandler<AddImageProcessCommand>,
        IAsyncNotificationHandler<AddImageProcessAnalyzeCommand>,
        IAsyncNotificationHandler<UpdateImageProcessAnalyzeCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IImageProcessRepository _imageProcessRepository;

        public ImageProcessCommandHandler(IMediatorHandler mediator,
                                          IImageProcessRepository imageProcessRepository,
                                          ILogger<ImageProcessCommandHandler> logger,
                                          INotificationHandler<DomainNotification> notifications)
            : base(mediator, (DomainNotificationHandler)notifications, logger)
        {
            _mediator = mediator;
            _imageProcessRepository = imageProcessRepository;
        }

        public async Task Handle(AddImageProcessCommand notification)
        {
            var entityCurrent = Mapper.Map<ImageProcessModel, ImageProcess>(notification.ImageProcessModel);

            if (!IsEntityValid(entityCurrent)) return;

            await _imageProcessRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new ImageProcessAddedEvent(_logger, Mapper.Map<ImageProcessModel>(notification.ImageProcessModel)));
        }

        public async Task Handle(AddImageProcessAnalyzeCommand notification)
        {
            var entityCurrent = Mapper.Map<ImageProcessAnalyzeModel, ImageProcessAnalyze>(notification.ImageProcessAnalyzeModel);

            if (!IsEntityValid(entityCurrent)) return;

            await _imageProcessRepository.AddAnalyzeAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new ImageProcessAnalyzeAddedEvent(_logger, Mapper.Map<ImageProcessAnalyzeModel>(notification.ImageProcessAnalyzeModel)));
        }

        public async Task Handle(UpdateImageProcessAnalyzeCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<ImageProcessAnalyzeModel, ImageProcessAnalyze>(notification.ImageProcessAnalyzeModel);
            if (entityCurrentNotification.Id == 0)
            {
                NotifyErrorValidations("ImageProcessAnalyze", "ImageProcessAnalyze.Id.GreaterThanZero");
                return;
            }
            if (!IsEntityValid(entityCurrentNotification)) return;

            var entityCurrentDB = await _imageProcessRepository.GetByIdAsync(notification.ImageProcessAnalyzeModel.Id);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "ImageProcessAnalyze.NotFound")) return;

            await _imageProcessRepository.UpdateAnalyzeAsync(entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new ImageProcessAnalyzeUpdatedEvent(_logger, Mapper.Map<ImageProcessAnalyzeModel>(notification.ImageProcessAnalyzeModel)));
        }
    }
}
