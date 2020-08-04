using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Trades.Models;

namespace CG4U.Donate.Domain.Trades.Commands
{
    public class AddTradeLocationCommand : Command
    {
        public TradeLocationModel TradeLocationModel { get; set; }

        public AddTradeLocationCommand(User userLoggedIn, TradeLocationModel tradeLocationModel)
            : base(userLoggedIn)
        {
            TradeLocationModel = tradeLocationModel;
        }
    }
}
