using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.ImageProcess.Models;

namespace CG4U.Security.Domain.ImageProcess.Commands
{
    public class AddImageProcessCommand : Command
    {
        public ImageProcessModel ImageProcessModel { get; set; }

        public AddImageProcessCommand(User userLoggedIn, ImageProcessModel imageProcessModel)
            : base(userLoggedIn)
        {
            ImageProcessModel = imageProcessModel;
        }
    }
}
