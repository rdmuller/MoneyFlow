using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain;
using Shared.Presentation.APIs.Bindings;

namespace Shared.Presentation.APIs.Models;

[ModelBinder(BinderType = typeof(QueryParamsBinder))]
public class BoundQueryParams : QueryParams
{
    public static ValueTask<BoundQueryParams> BindAsync(HttpContext context)
    {
        IQueryCollection query = context.Request.Query;

        BoundQueryParams result = new()
        {
            PageNum = query.TryGetValue("pageNum", out Microsoft.Extensions.Primitives.StringValues pageNum) && int.TryParse(pageNum, out int pNum) ? pNum : null,
            PageRows = query.TryGetValue("pageRows", out Microsoft.Extensions.Primitives.StringValues pageRows) && int.TryParse(pageRows, out int pRows) ? pRows : null,
            Sort = query.TryGetValue("sort", out Microsoft.Extensions.Primitives.StringValues sortValue) ? sortValue.ToString() : string.Empty,
            Status = query.TryGetValue("status", out Microsoft.Extensions.Primitives.StringValues statusValue) ? statusValue.ToString() : null,
            ExtraParams = query
                .Where(kv => !kv.Key.Equals("pageNum", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("pageRows", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("status", StringComparison.OrdinalIgnoreCase) &&
                             !kv.Key.Equals("sort", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kv => kv.Key, kv => kv.Value.ToString())
        };

        return ValueTask.FromResult(result);
    }
}
