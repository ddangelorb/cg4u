using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Donate.Domain.Trades.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Trades.Events
{
    public class TradeEvaluationAddedEvent : Event
    {
        public TradeEvaluationModel TradeEvaluationModel { get; set; }

        public TradeEvaluationAddedEvent(ILogger logger, TradeEvaluationModel tradeEvaluationModel)
            : base(logger)
        {
            TradeEvaluationModel = tradeEvaluationModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ TradeEvaluation Event Added successfully. TradeEvaluationModel.Id {1}",
                    DateTime.Now.ToString("G"), TradeEvaluationModel.Id
                )
            );
        }
    }
}
