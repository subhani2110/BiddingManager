using System.Net.Mail;
using System.Net;

namespace DataLayer.Services
{
    public class EmailService 
    {
        private readonly SmtpClient _client;

        public EmailService(string host, int port, string username, string password)
        {
            _client = new SmtpClient(host, port);
            _client.Credentials = new NetworkCredential(username, password);
            _client.EnableSsl = true;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MailMessage("noreply@example.com", to, subject, body);
            await _client.SendMailAsync(message);
        }
    }

}
