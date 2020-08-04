using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.ImageProcess.Models;

namespace CG4U.Security.Domain.ImageProcess.Commands
{
    public class AddImageProcessAnalyzeCommand : Command
    {
        public ImageProcessAnalyzeModel ImageProcessAnalyzeModel { get; set; }

        public AddImageProcessAnalyzeCommand(User userLoggedIn, ImageProcessAnalyzeModel imageProcessAnalyzeModel)
            : base(userLoggedIn)
        {
            ImageProcessAnalyzeModel = imageProcessAnalyzeModel;
        }
    }
}
