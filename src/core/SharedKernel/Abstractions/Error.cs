namespace SharedKernel.Abstractions;

public record Error(string Code, string Message)
{
    public static Error None => new(string.Empty, string.Empty);

    public static Error NullValue => new("Error.NullValue", "The value cannot be null.");

    public static Error DataTagNotFound => new("DataTagNotFound", "Tag 'Data' not found");

    public static Error RequiredFieldIsEmpty(string message) => new("RequiredFieldIsEmpty", message);

    public static Error InactiveForeignKey(string message) => new("InactiveForeignKey", message);

    public static Error NotAuthorized(string errorMessage) => new("NotAuthorized", errorMessage);

    public static Error ValidationError(string errorMessage) => new("ValidationError", errorMessage);

    public static Error RecordAlreadyExists(string errorMessage) => new("RecordAlreadyExists", errorMessage);

    public static Error RecordNotFound(string errorMessage) => new("RecordNotFound", errorMessage);
}
