using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Shared.Presentation.APIs.Models;

namespace Shared.Presentation.APIs.Bindings;

public class QueryParamsBinderProvider : IModelBinderProvider
{
    // caso quisesse configurar o binder, hoje isso não é usado
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(BoundQueryParams))
            return new BinderTypeModelBinder(typeof(QueryParamsBinder));

        return null;
    }
}
