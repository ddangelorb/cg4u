using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Trades.Models;

namespace CG4U.Donate.Domain.Trades.Commands
{
    public class AddTradeCommand : Command
    {
        public TradeModel TradeModel { get; set; }

        public AddTradeCommand(User userLoggedIn, TradeModel tradeModel)
            : base(userLoggedIn)
        {
            TradeModel = tradeModel;
        }
    }
}
