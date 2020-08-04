using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace CG4U.Core.Common.Domain.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public virtual bool HasNotifications()
        {
            return _notifications.Any();
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Handle(DomainNotification notification)
        {
            _notifications.Add(notification);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro: {notification.Key} - {notification.Value}");
        }
    }
}
