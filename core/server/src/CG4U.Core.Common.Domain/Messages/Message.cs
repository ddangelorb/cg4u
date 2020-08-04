using System;
using MediatR;

namespace CG4U.Core.Common.Domain.Messages
{
    public abstract class Message : INotification
    {
        public string MessageType { get; protected set; }
        public int AggregateId { get; protected set; }
        public DateTime Timestamp { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
            Timestamp = DateTime.Now;
        }
    }
}
