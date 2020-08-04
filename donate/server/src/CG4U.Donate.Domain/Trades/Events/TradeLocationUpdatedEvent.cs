using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Donate.Domain.Trades.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Trades.Events
{
    public class TradeLocationUpdatedEvent : Event
    {
        public TradeLocationModel TradeLocationModel { get; set; }

        public TradeLocationUpdatedEvent(ILogger logger, TradeLocationModel tradeLocationModel)
            : base(logger)
        {
            TradeLocationModel = tradeLocationModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ TradeLocation Event Updated successfully. TradeLocationModel.Id {1}",
                    DateTime.Now.ToString("G"), TradeLocationModel.Id
                )
            );
        }
    }
}
