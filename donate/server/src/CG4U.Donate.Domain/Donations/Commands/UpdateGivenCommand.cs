using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Donations.Models;

namespace CG4U.Donate.Domain.Donations.Commands
{
    public class UpdateGivenCommand : Command
    {
        public GivenModel GivenModel { get; set; }

        public UpdateGivenCommand(User userLoggedIn, GivenModel givenModel)
            : base(userLoggedIn)
        {
            GivenModel = givenModel;
        }
    }
}
