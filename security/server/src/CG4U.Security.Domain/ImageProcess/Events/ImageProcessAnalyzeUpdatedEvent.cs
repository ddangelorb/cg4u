using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.ImageProcess.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.ImageProcess.Events
{
    public class ImageProcessAnalyzeUpdatedEvent : Event
    {
        public ImageProcessAnalyzeModel ImageProcessAnalyzeModel { get; set; }

        public ImageProcessAnalyzeUpdatedEvent(ILogger logger, ImageProcessAnalyzeModel imageProcessAnalyzeModel)
            : base(logger)
        {
            ImageProcessAnalyzeModel = imageProcessAnalyzeModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ ImageProcess.Analyze Updated successfully. ImageProcessAnalyzeModel.Id {1}, ImageProcessAnalyzeModel.IdAnalyzeRequestVideoCameras {2}, ImageProcess.Analyze.DtAnalyze {3}",
                    DateTime.Now.ToString("G"), ImageProcessAnalyzeModel.Id, ImageProcessAnalyzeModel.IdAnalyzesRequestsVideoCameras, ImageProcessAnalyzeModel.DtAnalyze.ToString("G")
                )
            );
        }
    }
}
