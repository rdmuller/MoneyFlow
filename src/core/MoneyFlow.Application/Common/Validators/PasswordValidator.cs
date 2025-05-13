using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace MoneyFlow.Application.Common.Validators;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "PasswordValidator";

    private const string ERROR_CODE = "WeakdPassword";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_CODE}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password)
            || password.Length < 8
            || !Regex.IsMatch(password, @"[A-Z]+")
            || !Regex.IsMatch(password, @"[a-z]+")
            || !Numbers().IsMatch(password)
            || !SpecialSymbols().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_CODE, "Password must contain 8 characters, upper/lower case letters, numbers and special characters"); // Senha deve conter 8 caracteres, letras maiúsculas/minúsculas, números e caracteres especiais.
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[\!\?\*\.\+\-\@]+")]
    private static partial Regex SpecialSymbols();

    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Numbers();
}
