using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Persons.Models;

namespace CG4U.Security.Domain.Persons.Commands
{
    public class AddPersonGroupCommand : Command
    {
        public PersonGroupModel PersonGroupModel;

        public AddPersonGroupCommand(User userLoggedIn, PersonGroupModel personGroupModel)
            : base(userLoggedIn)
        {
            PersonGroupModel = personGroupModel;
        }
    }
}
