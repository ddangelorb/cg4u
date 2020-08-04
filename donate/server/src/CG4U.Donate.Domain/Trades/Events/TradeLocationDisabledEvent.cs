using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Trades.Events
{
    public class TradeLocationDisabledEvent : Event
    {
        public int IdTradeLocations { get; set; }

        public TradeLocationDisabledEvent(ILogger logger, int idTradeLocations)
            : base(logger)
        {
            IdTradeLocations = idTradeLocations;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ TradeLocation Event Disabled successfully. TradeLocation.Id {1}",
                    DateTime.Now.ToString("G"), IdTradeLocations
                )
            );
        }
    }
}
