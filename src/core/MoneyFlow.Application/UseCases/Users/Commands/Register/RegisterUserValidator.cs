using FluentValidation;
using MoneyFlow.Application.DTOs.Users;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommandDTO>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithMessage("Name is required.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .MinimumLength(5)
            .WithMessage("Name must be between 5 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail is required.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .EmailAddress()
            .WithMessage("E-mail must be valid.");
    }
}