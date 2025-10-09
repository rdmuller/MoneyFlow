using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.Common.Validators;

namespace MoneyFlow.Application.UseCases.Common.Users.Commands.ChangePassword;

internal class UserChangePasswordValidator : CommonValidator<string>
{
    public UserChangePasswordValidator()
    {
        RuleFor(x => x).SetValidator(new PasswordValidator<string>());
    }
}
