using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.Domain.Configuration.Commands
{
    public class AddAnalyzeRequestConnectionVideoCameraCommand : Command
    {
        public int IdAnalyzesRequests { get; set; }
        public int IdVideoCameras { get; set; }

        public AddAnalyzeRequestConnectionVideoCameraCommand(User userLoggedIn, int idAnalyzesRequests, int idVideoCameras)
            : base(userLoggedIn)
        {
            IdAnalyzesRequests = idAnalyzesRequests;
            IdVideoCameras = idVideoCameras;
        }
    }
}
