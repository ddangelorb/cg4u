using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.Domain.Trades.Commands
{
    public class DisableTradeCommand : Command
    {
        public int IdTrades { get; set; }

        public DisableTradeCommand(User userLoggedIn, int idTrades)
            : base(userLoggedIn)
        {
            IdTrades = idTrades;
        }
    }
}
