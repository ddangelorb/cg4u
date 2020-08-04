using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Configuration.Events
{
    public class AnalyzeRequestConnectionVideoCameraAddedEvent : Event
    {
        public int IdAnalyzesRequests { get; set; }
        public int IdVideoCameras { get; set; }

        public AnalyzeRequestConnectionVideoCameraAddedEvent(ILogger logger, int idAnalyzesRequests, int idVideoCameras)
            : base(logger)
        {
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                $"{DateTime.Now.ToString("G")} __ AnalyzeRequest Connection VideoCamera Added successfully. IdAnalyzesRequests {IdAnalyzesRequests}, IdVideoCameras {IdVideoCameras}"
            );
        }
    }
}
