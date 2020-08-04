using Microsoft.Extensions.Logging;

namespace CG4U.Core.Common.Domain.Messages
{
    public abstract class Event : Message
    {
        public ILogger Logger { get; protected set; }

        public Event(ILogger logger)
        {
            Logger = logger;
        }

        public abstract void NotifyEventHandled();
    }
}
