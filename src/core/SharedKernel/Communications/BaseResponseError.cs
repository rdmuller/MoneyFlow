using SharedKernel.Abstractions;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SharedKernel.Communications;

public class BaseResponseError : BaseResponseGeneric<string>
{
    [JsonIgnore]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new string? Data { get; set; }

    public BaseResponseError(IEnumerable<BaseError> errors)
    {
        Errors = errors;
        Data = null;
    }

    public BaseResponseError(Error error)
    {
        Errors = [BaseError.CreateError(error)];
    }

    public BaseResponseError(List<Error> errors)
    {
        Errors = errors.Select(e => BaseError.CreateError(e)).ToList();
    }
}