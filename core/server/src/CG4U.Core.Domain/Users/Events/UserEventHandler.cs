using MediatR;

namespace CG4U.Core.Domain.Users.Events
{
    public class UserEventHandler:
        INotificationHandler<UserAddedEvent>,
        INotificationHandler<UserEnabledEvent>,
        INotificationHandler<UserDisabledEvent>,
        INotificationHandler<UserUpdatedEvent>,
        INotificationHandler<SystemUserAddedEvent>
    {
        public void Handle(UserAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(UserEnabledEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(UserDisabledEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(UserUpdatedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(SystemUserAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }
    }
}
