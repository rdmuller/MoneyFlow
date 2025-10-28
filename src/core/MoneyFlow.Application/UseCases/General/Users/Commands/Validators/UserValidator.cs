using FluentValidation;
using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.Common.Validators;
using MoneyFlow.Application.DTOs.Common.Users;
using MoneyFlow.Domain.General.Entities.Users;

namespace MoneyFlow.Application.UseCases.General.Users.Commands.Validators;

internal class UserValidator : CommonValidator<User>
{
    public UserValidator()
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