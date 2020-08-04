using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.Configuration.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Configuration.Events
{
    public class VideoCameraAddedEvent : Event
    {
        public VideoCameraModel VideoCameraModel { get; set; }

        public VideoCameraAddedEvent(ILogger logger, VideoCameraModel videoCameraModel)
            : base(logger)
        {
            VideoCameraModel = videoCameraModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                $"{DateTime.Now.ToString("G")} __ VideoCamera Added successfully. VideoCameraModel.Id {VideoCameraModel.Id}, VideoCameraModel.IdPersonGroups { VideoCameraModel.IdPersonGroups }, VideoCameraModel.IdPersonGroupsAPI { VideoCameraModel.IdPersonGroupsAPI }, VideoCameraModel.Name { VideoCameraModel.Name }, VideoCameraModel.Description { VideoCameraModel.Description }"
            );
        }
    }
}
