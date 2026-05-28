using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoneyFlow.Presentation.APIs.Models;

namespace MoneyFlow.Presentation.APIs.Bindings;

public class QueryParamsBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        IQueryCollection query = bindingContext.HttpContext.Request.Query;

        BoundQueryParams result = new()
        {
            PageNum = TryGetInt(query, "pageNum"),
            PageRows = TryGetInt(query, "pageRows"),
            Sort = query.TryGetValue("sort", out Microsoft.Extensions.Primitives.StringValues sortValue) ? sortValue.ToString() : string.Empty,
            Status = query.TryGetValue("status", out Microsoft.Extensions.Primitives.StringValues statusValue) ? statusValue.ToString() : null,
            ExtraParams = query
                .Where(kv => !kv.Key.Equals("pageNum", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("pageRows", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("status", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("sort", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kv => kv.Key, kv => kv.Value.ToString())
        };

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }

    private static int? TryGetInt(IQueryCollection query, string key)
    {
        if (query.TryGetValue(key, out Microsoft.Extensions.Primitives.StringValues value) && int.TryParse(value, out int intValue))
            return intValue;

        return null;
    }
}
