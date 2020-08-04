using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.ImageProcess.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.ImageProcess.Events
{
    public class ImageProcessAddedEvent : Event
    {
        public ImageProcessModel ImageProcessModel { get; set; }

        public ImageProcessAddedEvent(ILogger logger, ImageProcessModel imageProcessModel)
            : base(logger)
        {
            ImageProcessModel = imageProcessModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ ImageProcess Added successfully. ImageProcess.Id {1}",
                    DateTime.Now.ToString("G"), ImageProcessModel.Id
                )
            );

            foreach (var a in ImageProcessModel.Analyzes)
            {
                Logger.LogInformation(
                    string.Format(
                        "{0} __ ImageProcess.Analyze Added successfully. ImageProcess.Id {1}, ImageProcess.Analyze.Id {2}, ImageProcess.Analyze.IdAnalyzeRequestVideoCameras {3}, ImageProcess.Analyze.DtAnalyze {4}",
                        DateTime.Now.ToString("G"), ImageProcessModel.Id, a.Id, a.IdAnalyzesRequestsVideoCameras, a.DtAnalyze.ToString("G")
                    )
                );
            }
        }
    }
}
