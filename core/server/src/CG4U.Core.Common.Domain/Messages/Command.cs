using CG4U.Core.Common.Domain.Models;

namespace CG4U.Core.Common.Domain.Messages
{
    public abstract class Command : Message
    {
        public User UserLoggedIn { get; protected set; }

        public Command(User userLoggedIn)
        {
            UserLoggedIn = userLoggedIn;
        }
    }
}
