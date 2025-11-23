using System.Text.Json.Serialization;

namespace SharedKernel.Communications;

public class BaseError
{
    [JsonPropertyName("error_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("error_message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("error_description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorDescription { get; set; }

    public BaseError()
    {
        
    }

    public BaseError(string errorCode, string errorMessage, string errorDescription)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        ErrorDescription = errorDescription;
    }

    public BaseError(string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public static BaseError RecordAlreadyExists(string errorMessage) => new BaseError() 
    { 
        ErrorCode = "RecordAlreadyExists", 
        ErrorMessage = errorMessage
    };

    public static BaseError Unauthorized(string errorMessage) => new BaseError()
    {
        ErrorCode = "Unauthorized",
        ErrorMessage = errorMessage
    };

    public static BaseError ValidationError(string errorMessage) => new BaseError()
    {
        ErrorCode = "ValidationError",
        ErrorMessage = errorMessage
    };
}