using MediatR;

namespace CG4U.Donate.Domain.Trades.Events
{
    public class TradeEventHandler :
        INotificationHandler<TradeAddedEvent>,
        INotificationHandler<TradeDisabledEvent>,
        INotificationHandler<TradeUpdatedEvent>,
        INotificationHandler<TradeEvaluationAddedEvent>,
        INotificationHandler<TradeEvaluationUpdatedEvent>,
        INotificationHandler<TradeLocationAddedEvent>,
        INotificationHandler<TradeLocationDisabledEvent>,
        INotificationHandler<TradeLocationUpdatedEvent>
    {
        public void Handle(TradeAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(TradeDisabledEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(TradeUpdatedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(TradeEvaluationAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(TradeEvaluationUpdatedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(TradeLocationAddedEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(TradeLocationDisabledEvent notification)
        {
            notification.NotifyEventHandled();
        }

        public void Handle(TradeLocationUpdatedEvent notification)
        {
            notification.NotifyEventHandled();
        }
    }
}
