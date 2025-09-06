using MimeKit;
using System.Net;
using System.Net;
using System.Net.Mail;
namespace efcoreApi.Services
{
    public class TripSender
    {
        // Our private configuration variables
        //private string host;
        //private int port;
        //private bool enableSSL;
        //private string senderName;
        //private string userName;
        //private string password;

        //// Get our parameterized configuration
        //public TripSender(string host, int port, bool enableSSL, string senderName, string userName, string password)
        //{
        //    this.host = host;
        //    this.port = port;
        //    this.enableSSL = enableSSL;
        //    this.userName = userName;
        //    this.password = password;
        //    this.senderName = senderName;
        //}

        // Use our configuration to send the email by using SmtpClient
        //public Task SendEmailAsync(string email, string subject, string htmlMessage)
        //{
        //    var client = new SmtpClient(host, port)
        //    {

        //        Credentials = new NetworkCredential(userName, password),
        //        EnableSsl = enableSSL
        //    };
        //    return client.SendMailAsync(
        //        new MailMessage(userName, email, subject, htmlMessage) { IsBodyHtml = true }
        //    );
        //}
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("tripmail", "5f8acf9759fbf5"));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            //emailMessage.Body = new TextPart("plain") { Text = message };
            // Create a multipart/mixed container for the body and attachments
            var builder = new BodyBuilder();

            // Add the email body (plain text or HTML)
            builder.TextBody = message;
            // Or for HTML: builder.HtmlBody = "<h1>This is an HTML email!</h1>";

            // Add attachments
            // Example 1: Attaching a file from a file path
            //var dir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");
            //builder.Attachments.Add(dir + "/Ticketing System.docx");
            //builder.Attachments.Add(dir + "/CV-2025.docx");

            // Example 2: Attaching a file from a stream
            //using (var stream = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "Ticketing System.docx")))
            //{
            //    builder.Attachments.Add("another_file.docx", stream);
            //}

            // Set the message body
            emailMessage.Body = builder.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("sandbox.smtp.mailtrap.io", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("5f8acf9759fbf5", "7bf4538409090a");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task sendMail(string mailTo,string subject ,string message)
        {
            // Looking to send emails in production? Check out our Email API/SMTP product!
   

            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("5f8acf9759fbf5", "7bf4538409090a"),
                EnableSsl = true
            };
            client.Send("from@example.com", mailTo, subject, message);
            System.Console.WriteLine("Sent");
        }

    }
}
