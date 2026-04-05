using System.Diagnostics;

namespace SharedKernel.Abstractions;

public record Error(string Code, string Message)
{
    public static Error None => new(string.Empty, string.Empty);

    public static Error NullValue => new("Error.NullValue", "The value cannot be null.");

    public static Error RequiredFieldisEmpty(string message) => new("RequiredFieldIsEmpty", message);

    public static Error InactiveForeignKey(string message) => new("InactiveForeignKey", message);
}
