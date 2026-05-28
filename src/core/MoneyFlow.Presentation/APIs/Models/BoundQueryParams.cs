using Microsoft.AspNetCore.Mvc;
using MoneyFlow.Presentation.APIs.Bindings;
using Shared.Domain;

namespace MoneyFlow.Presentation.APIs.Models;

[ModelBinder(BinderType = typeof(QueryParamsBinder))]
public class BoundQueryParams : QueryParams
{
}
