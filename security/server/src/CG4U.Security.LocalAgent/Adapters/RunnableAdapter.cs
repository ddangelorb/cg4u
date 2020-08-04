using System;
using CG4U.Security.LocalAgent.Datas;
using CG4U.Security.LocalAgent.Services;
using Microsoft.Extensions.Logging;

namespace CG4U.Security.LocalAgent.Adapters
{
    public abstract class RunnableAdapter
    {
        private EmailSenderData _emailSenderData;
        protected volatile bool _isRunning;
        protected readonly ILogger _logger;
        protected VideoCameraData _videoCameraData;

        public RunnableAdapter(EmailSenderData emailSenderData, ILogger logger, VideoCameraData videoCameraData)
        {
            _isRunning = true;
            _emailSenderData = emailSenderData;
            _logger = logger;
            _videoCameraData = videoCameraData;
        }

        public void StopRun()
        {
            _isRunning = false;
        }

        protected void ReportError(Exception ex, string title, string message)
        {
            _logger.LogError(ex, message);

            var emailSender = new EmailSender(_emailSenderData);
            emailSender.Send(title, message);
        }
    }
}
