using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoneyFlow.API.APIs.Models;

namespace MoneyFlow.API.APIs.Bindings;

public class QueryParamsBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var query = bindingContext.HttpContext.Request.Query;

        var result = new BoundQueryParams
        {
            PageNum = TryGetInt(query, "pageNum"),
            PageRows = TryGetInt(query, "pageRows"),
            Status = query.TryGetValue("status", out var statusValue) ? statusValue.ToString() : null,
            ExtraParams = query
                .Where(kv => !kv.Key.Equals("pageNum", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("pageRows", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("status", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kv => kv.Key, kv => kv.Value.ToString())
        };

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }

    private int? TryGetInt(IQueryCollection query, string key)
    {
        if (query.TryGetValue(key, out var value) && int.TryParse(value, out var intValue))
            return intValue;

        return null;
    }
}
