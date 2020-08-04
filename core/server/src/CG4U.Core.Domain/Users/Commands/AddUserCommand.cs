using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Domain.Users.Models;

namespace CG4U.Core.Domain.Users.Commands
{
    public class AddUserCommand : Command
    {
        public UserModel UserModel { get; set; }

        public AddUserCommand(User userLoggedIn, UserModel userModel)
            : base(userLoggedIn)
        {
            UserModel = userModel;
        }
    }
}
