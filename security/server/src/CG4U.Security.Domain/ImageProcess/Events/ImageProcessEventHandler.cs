using MediatR;

namespace CG4U.Security.Domain.ImageProcess.Events
{
    public class ImageProcessEventHandler :
        INotificationHandler<ImageProcessAddedEvent>,
        INotificationHandler<ImageProcessAnalyzeAddedEvent>,
        INotificationHandler<ImageProcessAnalyzeUpdatedEvent>
    {
        public void Handle(ImageProcessAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(ImageProcessAnalyzeAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(ImageProcessAnalyzeUpdatedEvent notification)
        {
            notification.NotifyEventHandled();
        }
    }
}
