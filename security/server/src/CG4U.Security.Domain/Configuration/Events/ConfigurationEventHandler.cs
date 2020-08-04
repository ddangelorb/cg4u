using MediatR;

namespace CG4U.Security.Domain.Configuration.Events
{
    public class ConfigurationEventHandler:
        INotificationHandler<AlertAddedEvent>,
        INotificationHandler<AlertConnectionPersonGroupAddedEvent>,
        INotificationHandler<AnalyzeRequestAddedEvent>,
        INotificationHandler<AnalyzeRequestConnectionVideoCameraAddedEvent>,
        INotificationHandler<VideoCameraAddedEvent>
    {
        public void Handle(AlertAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(AlertConnectionPersonGroupAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(AnalyzeRequestAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(AnalyzeRequestConnectionVideoCameraAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(VideoCameraAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }
    }
}
