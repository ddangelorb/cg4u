using MediatR;

namespace CG4U.Security.Domain.Persons.Events
{
    public class PersonEventHandler :
        INotificationHandler<PersonAddedEvent>,
        INotificationHandler<PersonGroupAddedEvent>,
        INotificationHandler<PersonFaceAddedEvent>
    {
        public void Handle(PersonAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(PersonGroupAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(PersonFaceAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }
    }
}
