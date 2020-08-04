using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Donate.Domain.Trades.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Trades.Events
{
    public class TradeEvaluationUpdatedEvent : Event
    {
        public TradeEvaluationModel TradeEvaluationModel { get; set; }

        public TradeEvaluationUpdatedEvent(ILogger logger, TradeEvaluationModel tradeEvaluationModel)
            : base(logger)
        {
            TradeEvaluationModel = tradeEvaluationModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ TradeEvaluation Event Updated successfully. TradeEvaluationModel.Id {1}",
                    DateTime.Now.ToString("G"), TradeEvaluationModel.Id
                )
            );
        }
    }
}
