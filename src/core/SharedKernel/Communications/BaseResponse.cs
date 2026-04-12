using SharedKernel.Abstractions;

namespace SharedKernel.Communications;

public class BaseResponse<T> : BaseResponseGeneric<T>
{
    public static BaseResponse<T> CreateSuccessResponse(T? data, object? objectId = null)
    {
        return new BaseResponse<T>
        {
            Data = data,
            ObjectId = objectId,
        };
    }
    public static BaseResponse<T> CreateNewObjectIdResponse(object? objectId = null)
    {
        return new BaseResponse<T>
        {
            ObjectId = objectId
        };
    }

    public static BaseResponse<T> CreateFailureResponse(List<Error> errors) => new BaseResponse<T>
    {
        Errors = errors.Select(e => BaseError.CreateError(e)).ToList()
    };
}