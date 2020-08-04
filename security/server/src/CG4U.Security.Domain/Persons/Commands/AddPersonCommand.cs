using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Persons.Models;

namespace CG4U.Security.Domain.Persons.Commands
{
    public class AddPersonCommand : Command
    {
        public PersonModel PersonModel { get; set; }

        public AddPersonCommand(User userLoggedIn, PersonModel personModel)
            : base(userLoggedIn)
        {
            PersonModel = personModel;
        }
    }
}
