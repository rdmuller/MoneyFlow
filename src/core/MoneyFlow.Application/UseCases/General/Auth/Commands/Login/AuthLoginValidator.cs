using FluentValidation;
using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.Common.Validators;

namespace MoneyFlow.Application.UseCases.General.Auth.Commands.Login;

public class AuthLoginValidator : CommonValidator<AuthLoginCommand>
{
    public AuthLoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required");

        When(x => !string.IsNullOrWhiteSpace(x.Email),
            () => RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");

        When(x => !string.IsNullOrWhiteSpace(x.Password),
            () => RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator<AuthLoginCommand>()).WithMessage("Invalid password"));
    }
}