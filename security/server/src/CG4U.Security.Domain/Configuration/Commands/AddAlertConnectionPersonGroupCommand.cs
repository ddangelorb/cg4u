using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Configuration.Commands
{
    public class AddAlertConnectionPersonGroupCommand : Command
    {
        public int IdPersonGroups { get; set; }
        public int IdAlerts { get; set; }

        public AddAlertConnectionPersonGroupCommand(User userLoggedIn, int idPersonGroups, int idAlerts)
            : base(userLoggedIn)
        {
            IdPersonGroups = idPersonGroups;
            IdAlerts = idAlerts;
        }
    }
}
