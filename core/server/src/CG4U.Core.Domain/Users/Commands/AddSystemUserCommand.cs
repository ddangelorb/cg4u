using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Core.Domain.Users.Commands
{
    public class AddSystemUserCommand : Command
    {
        public string IdUserIdentity { get; set; }
        public int IdSystems { get; set; }

        public AddSystemUserCommand(User userLoggedIn, string idUserIdentity, int idSystems)
            : base(userLoggedIn)
        {
            IdUserIdentity = idUserIdentity;
            IdSystems = idSystems;
        }
    }
}
