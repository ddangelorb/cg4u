using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.Domain.Donations.Commands
{
    public class DisableDesiredCommand : Command
    {
        public int IdDonationsDesired { get; set; }

        public DisableDesiredCommand(User userLoggedIn, int idDonationsDesired)
            : base(userLoggedIn)
        {
            IdDonationsDesired = idDonationsDesired;
        }
    }
}
