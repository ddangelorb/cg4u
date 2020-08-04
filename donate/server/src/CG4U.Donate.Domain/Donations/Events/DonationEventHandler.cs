using MediatR;

namespace CG4U.Donate.Domain.Donations.Events
{
    public class DonationEventHandler :
        INotificationHandler<DesiredAddedEvent>,
        INotificationHandler<DesiredDisabledEvent>,
        INotificationHandler<DesiredUpdatedEvent>,
        INotificationHandler<GivenAddedEvent>,
        INotificationHandler<GivenDisabledEvent>,
        INotificationHandler<GivenUpdatedEvent>,
        INotificationHandler<GivenImageAddedEvent>
    {
        public void Handle(DesiredAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(DesiredDisabledEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(DesiredUpdatedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(GivenAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(GivenDisabledEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(GivenUpdatedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(GivenImageAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }
    }
}
