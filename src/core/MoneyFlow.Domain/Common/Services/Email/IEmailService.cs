namespace MoneyFlow.Domain.Common.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
}
