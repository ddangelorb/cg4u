using System.Net;
using System.Net.Mail;
using CG4U.Security.LocalAgent.Datas;

namespace CG4U.Security.LocalAgent.Services
{
    public class EmailSender
    {
        private EmailSenderData _emailSenderData;

        public EmailSender(EmailSenderData emailSenderData)
        {
            _emailSenderData = emailSenderData;
        }

        public void Send(string subject, string message)
        {
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _emailSenderData.Email,
                    Password = _emailSenderData.Password
                };

                client.Credentials = credential;
                client.Host = _emailSenderData.Host;
                client.Port = _emailSenderData.Port;
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(_emailSenderData.Email));
                    emailMessage.From = new MailAddress(_emailSenderData.Email);
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    client.Send(emailMessage);
                }
            }
        }
    }
}
