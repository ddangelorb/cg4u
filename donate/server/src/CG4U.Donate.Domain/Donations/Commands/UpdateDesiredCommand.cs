using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Donations.Models;

namespace CG4U.Donate.Domain.Donations.Commands
{
    public class UpdateDesiredCommand : Command
    {
        public DesiredModel DesiredModel { get; set; }

        public UpdateDesiredCommand(User userLoggedIn, DesiredModel desiredModel)
            : base(userLoggedIn)
        {
            DesiredModel = desiredModel;
        }
    }
}
