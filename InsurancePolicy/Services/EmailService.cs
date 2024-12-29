using MailKit.Net.Smtp;
using MimeKit;

namespace InsurancePolicy.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Update based on your SMTP provider
        private readonly int _smtpPort = 587; // Port for TLS
        private readonly string _smtpUsername = "chandrabosechiluka@gmail.com";
        private readonly string _smtpPassword = "iroqmbetgftppkrg"; // Use environment variables in production

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("e-Insurance Pvt. Ltd.", _smtpUsername));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await smtpClient.SendAsync(email);
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}