using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.Domain.Donations.Commands
{
    public class DisableGivenCommand : Command
    {
        public int IdDonationsGivens { get; set; }

        public DisableGivenCommand(User userLoggedIn, int idDonationsGivens)
            : base(userLoggedIn)
        {
            IdDonationsGivens = idDonationsGivens;
        }
    }
}
