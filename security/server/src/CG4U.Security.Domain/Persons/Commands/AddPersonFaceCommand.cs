using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Persons.Models;

namespace CG4U.Security.Domain.Persons.Commands
{
    public class AddPersonFaceCommand: Command
    {
        public FaceModel FaceModel { get; set; }

        public AddPersonFaceCommand(User userLoggedIn, FaceModel faceModel)
            : base(userLoggedIn)
        {
            FaceModel = faceModel;
        }
    }
}
