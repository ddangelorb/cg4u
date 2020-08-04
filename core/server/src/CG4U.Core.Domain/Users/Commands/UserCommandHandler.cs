using AutoMapper;
using CG4U.Core.Common.Domain.Handlers;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Common.Domain.Notifications;
using CG4U.Core.Domain.Users.Events;
using CG4U.Core.Domain.Users.Models;
using CG4U.Core.Domain.Users.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace CG4U.Core.Domain.Users.Commands
{
    public class UserCommandHandler : CommandHandler,
        IAsyncNotificationHandler<AddUserCommand>,
        IAsyncNotificationHandler<EnableUserCommand>,
        IAsyncNotificationHandler<DisableUserCommand>,
        IAsyncNotificationHandler<UpdateUserCommand>,
        IAsyncNotificationHandler<AddSystemUserCommand>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IUserRepository _userRepository;

        public UserCommandHandler(IMediatorHandler mediator,
                                  IUserRepository userRepository,
                                  ILogger<UserCommandHandler> logger,
                                  INotificationHandler<DomainNotification> notifications)
            : base(mediator, (DomainNotificationHandler)notifications, logger)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        public async Task Handle(AddUserCommand notification)
        {
            var entityCurrent = Mapper.Map<UserModel, User>(notification.UserModel);

            if (!IsEntityValid(entityCurrent)) return;

            if (!IsUsersValid(notification.UserLoggedIn, entityCurrent)) return;

            await _userRepository.AddAsync(entityCurrent);
            await _userRepository.AddSystemAsync(entityCurrent.IdUserIdentity , entityCurrent.IdSystems);

            if (Commit())
                await _mediator.PublishEvent(new UserAddedEvent(_logger, Mapper.Map<UserModel>(notification.UserModel)));
        }

        public async Task Handle(EnableUserCommand notification)
        {
            var entityCurrent = await _userRepository.GetByIdUserIdentityAsync(notification.IdUserIdentiy);

            if (!IsEntityFound(entityCurrent, notification.MessageType, "User.NotFound")) return;

            if (!IsUsersValid(notification.UserLoggedIn, entityCurrent)) return;

            await _userRepository.EnableByIdUserIdentityAsync(notification.IdUserIdentiy, notification.IdSystems);

            if (Commit())
                await _mediator.PublishEvent(new UserEnabledEvent(_logger, notification.IdUserIdentiy));
        }

        public async Task Handle(DisableUserCommand notification)
        {
            var entityCurrent = await _userRepository.GetByIdAsync(notification.IdUser);

            if (!IsEntityFound(entityCurrent, notification.MessageType, "User.NotFound")) return;

            if (!IsUsersValid(notification.UserLoggedIn, entityCurrent)) return;

            await _userRepository.DisableAsync(notification.IdUser);

            if (Commit())
                await _mediator.PublishEvent(new UserDisabledEvent(_logger, notification.IdUser));
        }

        public async Task Handle(UpdateUserCommand notification)
        {
            var entityCurrentNotification = Mapper.Map<UserModel, User>(notification.UserModel);

            var entityCurrentDB = await _userRepository.GetByIdAsync(notification.UserModel.Id);

            if (!IsEntityValid(entityCurrentNotification)) return;

            if (!IsEntityFound(entityCurrentDB, notification.MessageType, "User.NotFound")) return;

            if (!IsUsersValid(notification.UserLoggedIn, entityCurrentDB, entityCurrentNotification)) return;

            await _userRepository.UpdateAsync(entityCurrentNotification);

            if (Commit())
                await _mediator.PublishEvent(new UserUpdatedEvent(_logger, Mapper.Map<UserModel>(notification.UserModel)));
        }

        public async Task Handle(AddSystemUserCommand notification)
        {
            var entityCurrent = await _userRepository.GetByIdUserIdentityAsync(notification.IdUserIdentity);

            if (!IsEntityFound(entityCurrent, notification.MessageType, "User.NotFound")) return;

            if (!IsUsersValid(notification.UserLoggedIn, entityCurrent)) return;

            await _userRepository.AddSystemAsync(notification.IdUserIdentity, notification.IdSystems);

            if (Commit())
                await _mediator.PublishEvent(new SystemUserAddedEvent(_logger, notification.IdUserIdentity, notification.IdSystems));
        }
    }
}
