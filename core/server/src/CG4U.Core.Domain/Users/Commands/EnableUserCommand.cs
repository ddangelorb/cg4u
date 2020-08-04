using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Core.Domain.Users.Commands
{
    public class EnableUserCommand : Command
    {
        public string IdUserIdentiy { get; set; }
        public int IdSystems { get; set; }

        public EnableUserCommand(User userLoggedIn, string idUserIdentiy, int idSystems)
            : base(userLoggedIn)
        {
            IdUserIdentiy = idUserIdentiy;
            IdSystems = idSystems;
        }
    }
}
