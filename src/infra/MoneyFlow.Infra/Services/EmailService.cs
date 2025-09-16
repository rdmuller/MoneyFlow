using MoneyFlow.Domain.Services.Email;
using MoneyFlow.Infra.Settings;
using MoneyFlow.Common.Exceptions;
using System.Net.Mail;

namespace MoneyFlow.Infra.Services;

internal class EmailService(EmailSettings emailSettings) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings;

    public async Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        var jsonSettings = System.Text.Json.JsonSerializer.Serialize(_emailSettings);

        Console.WriteLine(jsonSettings);

        if (string.IsNullOrWhiteSpace(emailMessage.Body))
            throw new RequiredFieldIsEmptyException("Body of email is required");

        using var email = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
            Subject = emailMessage.Subject,
            IsBodyHtml = false,
            Body = emailMessage.Body,
        };

        foreach (var emailTo in emailMessage.To)
            email.Bcc.Add(emailTo);

        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
        {
            EnableSsl = _emailSettings.EnableSsl,
            Credentials = new System.Net.NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword)
        };

        await client.SendMailAsync(email, cancellationToken);

        return;
    }
}