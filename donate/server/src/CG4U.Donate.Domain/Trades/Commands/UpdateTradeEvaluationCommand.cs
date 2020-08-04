using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Trades.Models;

namespace CG4U.Donate.Domain.Trades.Commands
{
    public class UpdateTradeEvaluationCommand : Command
    {
        public TradeEvaluationModel TradeEvaluationModel { get; set; }

        public UpdateTradeEvaluationCommand(User userLoggedIn, TradeEvaluationModel tradeEvaluationModel)
            : base(userLoggedIn)
        {
            TradeEvaluationModel = tradeEvaluationModel;
        }
    }
}
