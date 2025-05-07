using FluentValidation;
using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.Common.Validators;
using MoneyFlow.Application.DTOs.Users;

namespace MoneyFlow.Application.UseCases.Users.Commands.Register;

public class RegisterUserValidator : Validator<RegisterUserCommandDTO>
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

            RuleFor(x => x.Password).SetValidator(new PasswordValidator<RegisterUserCommandDTO>());
    }

    /*public override async Task<ValidationResult> ValidateAsync(ValidationContext<RegisterUserCommandDTO> context, CancellationToken cancellation = default)
    {
        var result = await base.ValidateAsync(context, cancellation);
        if (!result.IsValid)
        {
            var failures = result
                .Errors
                .Select(e => new BaseError() { ErrorCode = e.ErrorCode, ErrorMessage = e.ErrorMessage })
                .ToList();
            throw new ErrorOnValidationException(failures);
        }

        return result;
    }*/
}