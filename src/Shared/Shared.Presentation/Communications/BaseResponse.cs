using System.Text.Json.Serialization;
using Shared.Application.Exceptions;
using Shared.Domain;

namespace Shared.Presentation.Communications;

public class BaseResponse<T>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("errors")]
    public IEnumerable<BaseError>? Errors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("object_id")]
    public object? ObjectId { get; set; }

    public static BaseResponse<T> CreateSuccessResponse(T? data, object? objectId = null) => new BaseResponse<T>
    {
        Data = data,
        ObjectId = objectId,
    };
    
    public static BaseResponse<T> CreateNewObjectIdResponse(object? objectId = null) => new BaseResponse<T>
    {
        ObjectId = objectId
    };

    public static BaseResponse<T> CreateFailureResponse(List<Error> errors) => new BaseResponse<T>
    {
        Errors = errors.Select(e => BaseError.CreateError(e)).ToList()
    };

    public static BaseResponse<T> CreateErrorResponse(Error error) => new BaseResponse<T>{
        Errors = [BaseError.CreateError(error)]
    };

    public static BaseResponse<T> CreateErrorResponse(List<Error> errors) => new BaseResponse<T>{
        Errors = errors.Select(e => BaseError.CreateError(e)).ToList()
    };
}
