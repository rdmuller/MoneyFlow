using SharedKernel.Communications;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MoneyFlow.Infra.DataAccess.Queries;
internal class QuerySpecification<T> : IQuerySpecification<T>
{
    public int Skip { get; private set; }
    public int Take { get; private set; }
    public List<Expression<Func<T, bool>>> Filters { get; set; } = new();

    private const string _ActivePropertyName = "Active";

    public QuerySpecification(QueryParams query)
    {
        Skip = (query.PageNum - 1) * query.PageRows ?? 0;
        Take = query.PageRows ?? 9999;

        var statusFilter = StatusFilterExtensions.FromCode(query.Status);
        if (!statusFilter.Equals(StatusFilter.Full) && typeof(T).GetProperty(_ActivePropertyName) is not null)
        {
            if (statusFilter.Equals(StatusFilter.Active)) // testar se possui essa propriedade
                Filters.Add(x => EF.Property<bool>(x!, _ActivePropertyName) == true);
            else
                Filters.Add(x => !EF.Property<bool>(x!, _ActivePropertyName) == false);
        }

        if (query.ExtraParams != null && query.ExtraParams.Any())
        {
            //foreach (var param in query.ExtraParams)
            //{
            //    var key = param.Key;
            //    var value = param.Value;
            //    Filters.Add(x => EF.Property<string>(x!, key) == value);
            //}
        }
    }
}
