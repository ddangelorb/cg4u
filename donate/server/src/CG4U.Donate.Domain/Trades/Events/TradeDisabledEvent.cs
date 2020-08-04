using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Trades.Events
{
    public class TradeDisabledEvent : Event
    {
        public int IdTrades { get; set; }

        public TradeDisabledEvent(ILogger logger, int idTrades)
            : base(logger)
        {
            IdTrades = idTrades;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Trade Event Disabled successfully. Trade.Id {1}",
                    DateTime.Now.ToString("G"), IdTrades
                )
            );
        }
    }
}
