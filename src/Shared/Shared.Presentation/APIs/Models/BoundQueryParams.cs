using Microsoft.AspNetCore.Mvc;
using Shared.Presentation.APIs.Bindings;
using Shared.Domain;

namespace Shared.Presentation.APIs.Models;

[ModelBinder(BinderType = typeof(QueryParamsBinder))]
public class BoundQueryParams : QueryParams
{
}
