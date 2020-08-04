using System;
using CG4U.Core.Common.Domain.Messages;
using CG4U.Security.Domain.Configuration.Models;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.Domain.Configuration.Events
{
    public class AnalyzeRequestAddedEvent : Event
    {
        public AnalyzeRequestModel AnalyzeRequestModel { get; set; }

        public AnalyzeRequestAddedEvent(ILogger logger, AnalyzeRequestModel analyzeRequestModel)
            : base(logger)
        {
            AnalyzeRequestModel = analyzeRequestModel;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                $"{DateTime.Now.ToString("G")} __ AnalyzeRequest Added successfully. AnalyzeRequestModel.Id {AnalyzeRequestModel.Id}, IdBillableProcesses {AnalyzeRequestModel.IdBillableProcesses}, IdLanguages {AnalyzeRequestModel.IdLanguages}, AnalyzeOrder {AnalyzeRequestModel.AnalyzeOrder}, TypeCode {AnalyzeRequestModel.TypeCode}, TypeName {AnalyzeRequestModel.TypeName}, IdAnalyzesRequestsVideoCameras {AnalyzeRequestModel.IdAnalyzesRequestsVideoCameras}, Location {AnalyzeRequestModel.Location}, SubscriptionKey {AnalyzeRequestModel.SubscriptionKey}"
            );
        }
    }
}
