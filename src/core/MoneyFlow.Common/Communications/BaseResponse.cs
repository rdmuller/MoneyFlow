using MoneyFlow.Common.Abstractions;

namespace MoneyFlow.Common.Communications;

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
}