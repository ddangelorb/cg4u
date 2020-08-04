using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Core.Domain.Users.Commands
{
    public class DisableUserCommand : Command
    {
        public int IdUser { get; set; }

        public DisableUserCommand(User userLoggedIn, int idUser)
            : base(userLoggedIn)
        {
            IdUser = idUser;
        }
    }
}
