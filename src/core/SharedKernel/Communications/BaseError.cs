using SharedKernel.Abstractions;
using System.Text.Json.Serialization;

namespace SharedKernel.Communications;

public class BaseError
{
    [JsonPropertyName("error_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorCode { get; private set; }

    [JsonPropertyName("error_message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; private set; }

    /*[JsonPropertyName("error_description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorDescription { get; private set; }*/

    public BaseError() { }

    public BaseError(Error error)
    {
        ErrorCode = error.Code;
        ErrorMessage = error.Message;
    }

    public static BaseError CreateError(Error error) => new BaseError(error);
}