using System.Linq.Expressions;

namespace MoneyFlow.Infra.DataAccess.Queries;
internal interface IQuerySpecification<T>
{
    int Skip { get; }
    int Take { get; }
    List<Expression<Func<T, bool>>> Filters { get; set; }
}
