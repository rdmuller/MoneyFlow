using Microsoft.AspNetCore.Mvc;
using MoneyFlow.API.APIs.Bindings;
using Shared.Domain;

namespace MoneyFlow.API.APIs.Models;

[ModelBinder(BinderType = typeof(QueryParamsBinder))]
public class BoundQueryParams : QueryParams
{
}
