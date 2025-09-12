using MoneyFlow.Domain.Services.Email;
using MoneyFlow.Infra.Settings;

namespace MoneyFlow.Infra.Services;

internal class EmailService(EmailSettings emailSettings) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings;

    public Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        var jsonSettings = System.Text.Json.JsonSerializer.Serialize(_emailSettings);

        Console.WriteLine(jsonSettings);

        if (string.IsNullOrWhiteSpace(emailMessage.Body))
            throw new 


        return Task.CompletedTask;
    }
}


/*
 
 using System.Net.Mail;
using NomeDoProjeto.Domain.Interfaces;
using NomeDoProjeto.Domain.Models;
using NomeDoProjeto.Infrastructure.Settings;

namespace NomeDoProjeto.Infrastructure.Services
{
    internal class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings ?? throw new ArgumentNullException(nameof(emailSettings));
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            if (emailMessage == null) throw new ArgumentNullException(nameof(emailMessage));

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = emailMessage.IsHtml
            };
            mailMessage.To.Add(emailMessage.To);

            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new System.Net.NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailsAsync(IEnumerable<EmailMessage> emailMessages)
        {
            if (emailMessages == null) throw new ArgumentNullException(nameof(emailMessages));

            using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new System.Net.NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };

            foreach (var emailMessage in emailMessages)
            {
                if (emailMessage == null) continue;

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    IsBodyHtml = emailMessage.IsHtml
                };
                mailMessage.To.Add(emailMessage.To);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
 
 */