using Microsoft.Extensions.Logging;
using MoneyFlow.Application.Common.Events;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Services.Email;

namespace MoneyFlow.Application.UseCases.General.Users.DomainEvents;

internal sealed class SendEmailChangePassword(IEmailService emailService) : IDomainEventHandler<UserChangePasswordDomainEvent>
{
    private readonly IEmailService _emailService = emailService;
    public async Task HandleAsync(UserChangePasswordDomainEvent notification, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Chegou no programa de envio de e-mail");

        await _emailService.SendEmailAsync(
            new EmailMessage
            {
                To = [notification.User.Email],
                Subject = "Password Change Notification",
                Body = $"Hello {notification.User.Name}, your password has been changed successfully."
            },
            cancellationToken
        );
    }
}
