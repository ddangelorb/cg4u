using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.Domain.Trades.Commands
{
    public class DisableTradeLocationCommand : Command
    {
        public int IdTradeLocations { get; set; }

        public DisableTradeLocationCommand(User userLoggedIn, int idTradeLocations)
            : base(userLoggedIn)
        {
            IdTradeLocations = idTradeLocations;
        }
    }
}
