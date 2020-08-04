using AutoMapper;
using CG4U.Donate.Domain.Donations.Repository;
using MediatR;
using CG4U.Donate.Domain.Donations.Events;
using CG4U.Donate.Domain.Donations.Models;
using Microsoft.Extensions.Logging;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Notifications;
using System.Threading.Tasks;

namespace CG4U.Donate.Domain.Donations.Commands
{
    public class DonationCommandHandler : CommandHandler,
        IAsyncNotificationHandler<AddDesiredCommand>,
        IAsyncNotificationHandler<AddGivenCommand>,
        IAsyncNotificationHandler<AddGivenImageCommand>,
        IAsyncNotificationHandler<DisableDesiredCommand>,
        IAsyncNotificationHandler<DisableGivenCommand>,
        IAsyncNotificationHandler<UpdateDesiredCommand>,
        IAsyncNotificationHandler<UpdateGivenCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IGivenRepository _givenRepository;
        private readonly IDesiredRepository _desiredRepository;

        public DonationCommandHandler(IMediatorHandler mediator,
                                      IGivenRepository givenRepository,
                                      IDesiredRepository desiredRepository,
                                      ILogger<DonationCommandHandler> logger,
                                      INotificationHandler<DomainNotification> notifications)
            : base(mediator, (DomainNotificationHandler)notifications, logger) 
        {
            _mediator = mediator;
            _givenRepository = givenRepository;
            _desiredRepository = desiredRepository;
        }

        public async Task Handle(AddDesiredCommand notification)
        {
            var entityCurrent = Mapper.Map<DesiredModel, Desired>(notification.DesiredModel);

            if (!IsEntityValid(entityCurrent) 
                || !IsUsersValid(notification.UserLoggedIn, entityCurrent.User)) 
                    return;

            await _desiredRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new DesiredAddedEvent(_logger, Mapper.Map<DesiredModel>(notification.DesiredModel)));
        }

        public async Task Handle(AddGivenCommand notification)
        {
            var entityCurrent = Mapper.Map<GivenModel, Given>(notification.GivenModel);

            if (!IsEntityValid(entityCurrent)
                || !IsUsersValid(notification.UserLoggedIn, entityCurrent.User))
                return;

            await _givenRepository.AddAsync(entityCurrent);

            if (Commit())
                await _mediator.PublishEvent(new GivenAddedEvent(_logger, Mapper.Map<GivenModel>(notification.GivenModel)));
        }

        public async Task Handle(DisableDesiredCommand notification)
        {
            var entityCurrent = await _desiredRepository.GetByIdAsync(notification.IdDonationsDesired);

            if (!IsEntityFound(entityCurrent, notification.MessageType, "DonationDesired.NotFound")) return;

            if (!IsUserCanModifyEntity(notification.UserLoggedIn, entityCurrent.User)) return;

            await _desiredRepository.DisableAsync(entityCurrent.Id);

            if (Commit())
                await _mediator.PublishEvent(new DesiredDisabledEvent(_logger, notification.IdDonationsDesired));
        }

        public async Task Handle(DisableGivenCommand notification)
        {
            var entityCurrent = await _givenRepository.GetByIdAsync(notification.IdDonationsGivens);

            if (!IsEntityFound(entityCurrent, notification.MessageType, "DonationGiven.NotFound")) return;

            if (!IsUserCanModifyEntity(notification.UserLoggedIn, entityCurrent.User)) return;

            await _givenRepository.DisableAsync(entityCurrent.Id);

            if (Commit())
                await _mediator.PublishEvent(new GivenDisabledEvent(_logger, notification.IdDonationsGivens));
        }

        public async Task Handle(UpdateDesiredCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<DesiredModel, Desired>(notification.DesiredModel);
            if (entityCurrentNotification.Id == 0)
            {
                NotifyErrorValidations("DonationDesired", "Desired.IdDonationsDesired.GreaterThanZero");
                return;
            }
            if (!IsEntityValid(entityCurrentNotification)) return;
            if (!IsUsersValid(notification.UserLoggedIn, entityCurrentNotification.User)) return;

            var entityCurrentDB = await _desiredRepository.GetByIdAsync(notification.DesiredModel.Donation.Id);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "DonationDesired.NotFound")) return;
            if (entityCurrentDB.User != entityCurrentNotification.User)
            {
                NotifyErrorValidations("DonationDesired", "DonationDesired.InvalidEntityUser");
                return;
            }

            await _desiredRepository.UpdateAsync(entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new DesiredUpdatedEvent(_logger, Mapper.Map<DesiredModel>(notification.DesiredModel)));
        }

        public async Task Handle(UpdateGivenCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<GivenModel, Given>(notification.GivenModel);
            if (entityCurrentNotification.Id == 0)
            {
                NotifyErrorValidations("DonationsGivens", "Given.IdDonationsGivens.GreaterThanZero");
                return;
            }
            if (!IsEntityValid(entityCurrentNotification)) return;
            if (!IsUsersValid(notification.UserLoggedIn, entityCurrentNotification.User)) return;

            var entityCurrentDB = await _givenRepository.GetByIdAsync(notification.GivenModel.Donation.Id);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "DonationGiven.NotFound")) return;
            if (entityCurrentDB.User != entityCurrentNotification.User)
            {
                NotifyErrorValidations("DonationsGivens", "DonationsGivens.InvalidEntityUser");
                return;
            }

            await _givenRepository.UpdateAsync(entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new GivenUpdatedEvent(_logger, Mapper.Map<GivenModel>(notification.GivenModel)));
        }

        public async Task Handle(AddGivenImageCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<GivenModel, Given>(notification.GivenModel);
            if (entityCurrentNotification.Id == 0)
            {
                NotifyErrorValidations("DonationsGivens", "Given.IdDonationsGivens.GreaterThanZero");
                return;
            }
            if (!IsEntityValid(entityCurrentNotification)) return;
            if (!IsUsersValid(notification.UserLoggedIn, entityCurrentNotification.User)) return;

            var entityCurrentDB = await _givenRepository.GetByIdAsync(notification.GivenModel.Donation.Id);
            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "DonationGiven.NotFound")) return;
            if (entityCurrentDB.User != entityCurrentNotification.User)
            {
                NotifyErrorValidations("DonationsGivens", "DonationsGivens.InvalidEntityUser");
                return;
            }

            await _givenRepository.AddImageAsync(entityCurrentNotification.Id, entityCurrentNotification.Img);

            if (Commit())
                await _mediator.PublishEvent(new GivenImageAddedEvent(_logger, Mapper.Map<GivenModel>(notification.GivenModel)));
        }
    }
}
