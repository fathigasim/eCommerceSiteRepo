using System.Net;
using System.Net.Mail;

namespace efcoreApi.Services
{
    public class EmailSender : IEmailSender
    {
        // Our private configuration variables
        //private string host;
        //private int port;
        //private bool enableSSL;
        //private string userName;
        //private string password;

        //// Get our parameterized configuration
        //public EmailSender(string host, int port, bool enableSSL, string userName, string password)
        //{
        //    this.host = host;
        //    this.port = port;
        //    this.enableSSL = enableSSL;
        //    this.userName = userName;
        //    this.password = password;
        //}
        private readonly EmailSettings _settings;

        public EmailSender(EmailSettings settings)
        {
            _settings = settings;
        }
        // Use our configuration to send the email by using SmtpClient
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_settings.Host, _settings.Port)
            {

                Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                EnableSsl = _settings.EnableSSL
            };
            return client.SendMailAsync(
                new MailMessage(_settings.UserName,email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}
