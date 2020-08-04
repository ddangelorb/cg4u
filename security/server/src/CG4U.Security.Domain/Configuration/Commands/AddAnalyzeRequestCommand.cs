using CG4U.Core.Common.Domain.Messages;
using CG4U.Core.Common.Domain.Models;
using CG4U.Security.Domain.Configuration.Models;

namespace CG4U.Security.Domain.Configuration.Commands
{
    public class AddAnalyzeRequestCommand : Command
    {
        public AnalyzeRequestModel AnalyzeRequestModel { get; set; }

        public AddAnalyzeRequestCommand(User userLoggedIn, AnalyzeRequestModel analyzeRequestModel)
            : base(userLoggedIn)
        {
            AnalyzeRequestModel = analyzeRequestModel;
        }
    }
}
