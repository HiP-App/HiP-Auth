using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;

namespace Auth.Utility
{
    public class EmailSender
    {
        private AppConfig appConfig;
        public EmailSender(AppConfig appConfig)
        {
            this.appConfig = appConfig;
        }
        public Task InviteAsync(string email, string password)
        {
            var smtp = appConfig.SMTPConfig;            
            var bodyHtml = System.IO.File.ReadAllText(Path.Combine("Utility", "invitation-forgot-password.html"));
            bodyHtml = bodyHtml.Replace(@"'Password'", password);
            bodyHtml = bodyHtml.Replace(@"{email}", email);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("History in Paderborn", smtp.From));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Neues Password für die History in Paderborn App";
            message.Body = new TextPart("html")
            {
                Text = bodyHtml
            };          

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(smtp.Server, smtp.Port, smtp.WithSSL);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Send(message);
                client.Disconnect(true);
            }

            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}
