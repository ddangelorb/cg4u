using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.Configuration.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Configuration.Events
{
    public class AlertAddedEvent : Event
    {
        public AlertModel AlertModel { get; set; }

        public AlertAddedEvent(ILogger logger, AlertModel alertModel)
            : base(logger)
        {
            AlertModel = alertModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                $"{DateTime.Now.ToString("G")} __ Alert Added successfully. AlertModel.Id {AlertModel.Id}, AlertModel.IdAnalyzesRequests {AlertModel.IdAnalyzesRequests}, AlertModel.TypeCode {AlertModel.TypeCode}, AlertModel.Message {AlertModel.Message}, AlertModel.ProcessingMethod {AlertModel.ProcessingMethod}, AlertModel.ProcessingParam {AlertModel.ProcessingParam}"
            );
        }
    }
}
