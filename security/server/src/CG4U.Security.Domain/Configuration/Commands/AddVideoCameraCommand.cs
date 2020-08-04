using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration.Models;

namespace CG4U.Security.Domain.Configuration.Commands
{
    public class AddVideoCameraCommand : Command
    {
        public VideoCameraModel VideoCameraModel { get; set; }

        public AddVideoCameraCommand(User userLoggedIn, VideoCameraModel videoCameraModel)
            : base(userLoggedIn)
        {
            VideoCameraModel = videoCameraModel;
        }
    }
}
