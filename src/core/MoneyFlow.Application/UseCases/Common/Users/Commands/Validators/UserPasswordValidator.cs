using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.Common.Validators;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.Validators;

internal class UserPasswordValidator : CommonValidator<string>
{
    public UserPasswordValidator()
    {
        RuleFor(x => x).SetValidator(new PasswordValidator<string>());
    }
}
