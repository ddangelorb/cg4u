using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration.Models;

namespace CG4U.Security.Domain.Configuration.Commands
{
    public class AddAlertCommand : Command
    {
        public AlertModel AlertModel { get; set; }

        public AddAlertCommand(User userLoggedIn, AlertModel alertModel)
            : base(userLoggedIn)
        {
            AlertModel = alertModel;
        }
    }
}
