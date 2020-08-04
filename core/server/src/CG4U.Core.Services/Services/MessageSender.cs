using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CG4U.Core.Services.Interfaces;

namespace CG4U.Core.Services.Services
{
    public class MessageSender : IEmailSender, ISmsSender
    {
        private bool _emailConfigOk;
        private string _emailUser;
        private string _emailPwd;
        private string _emailHost;
        private int _emailPort;
        private bool _smsConfigOk;

        public MessageSender()
        {
            _emailConfigOk = false;
            _smsConfigOk = false;
        }

        public MessageSender(string emailUser, string emailPwd, string emailHost, int emailPort)
        {
            _emailConfigOk = true;
            _emailUser = emailUser;
            _emailPwd = emailPwd;
            _emailHost = emailHost;
            _emailPort = emailPort;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (_emailConfigOk)
            {
                using (var client = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = _emailUser,
                        Password = _emailPwd
                    };

                    client.Credentials = credential;
                    client.Host = _emailHost;
                    client.Port = _emailPort;
                    client.EnableSsl = true;

                    using (var emailMessage = new MailMessage())
                    {
                        emailMessage.To.Add(new MailAddress(toEmail));
                        emailMessage.From = new MailAddress(_emailUser);
                        emailMessage.Subject = subject;
                        emailMessage.Body = message;
                        client.Send(emailMessage);
                    }
                }
                await Task.CompletedTask;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            if (_smsConfigOk)
                throw new NotImplementedException();
            return null;
        }
    }
}
