using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Donate.Domain.Trades.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Trades.Events
{
    public class TradeUpdatedEvent : Event
    {
        public TradeModel TradeModel { get; set; }

        public TradeUpdatedEvent(ILogger logger, TradeModel tradeModel)
            : base(logger)
        {
            TradeModel = tradeModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Trade Event Updated successfully. Trade.Id {1}, UserGet.Id {2}, UserLet.Id {3}",
                    DateTime.Now.ToString("G"), TradeModel.Id, TradeModel.UserGet.Id, TradeModel.UserLet.Id
                )
            );
        }
    }
}
