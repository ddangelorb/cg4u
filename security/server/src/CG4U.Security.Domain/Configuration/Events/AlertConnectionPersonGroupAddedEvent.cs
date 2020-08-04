using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Configuration.Events
{
    public class AlertConnectionPersonGroupAddedEvent : Event
    {
        public int IdPersonGroups { get; set; }
        public int IdAlerts { get; set; }

        public AlertConnectionPersonGroupAddedEvent(ILogger logger, int idPersonGroups, int idAlerts)
            : base(logger)
        {
            IdPersonGroups = idPersonGroups;
            IdAlerts = idAlerts;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                $"{DateTime.Now.ToString("G")} __ Alert Connection PersonGroup Added successfully. IdPersonGroups {IdPersonGroups}, IdAlerts {IdAlerts}"
            );
        }
    }
}
