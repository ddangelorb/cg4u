using System;
using System.Net;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CG4U.Core.Services.Handlers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private MessageSender _messageSender;

        public ErrorHandlerMiddleware(
                RequestDelegate next, 
                IOptions<EmailSender> emailSenderOption,
                ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _messageSender = new MessageSender(
                emailSenderOption.Value.Email,
                emailSenderOption.Value.Password,
                emailSenderOption.Value.Host,
                int.Parse(emailSenderOption.Value.Port)
            );
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);                
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var msgError = string.Concat(exception.Message, "\\n", exception.StackTrace);
                
            _logger.LogError(1, exception, msgError);

            await _messageSender.SendEmailAsync(
                "cg4io@outlook.com",
                "CG4U: Critical Error",
                msgError);

            var result = JsonConvert.SerializeObject(new { error = exception.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(result);
        }
    }
}
