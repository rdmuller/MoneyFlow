using SharedKernel.Communications;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MoneyFlow.Infra.DataAccess.Queries;
internal class QuerySpecification<T>
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
            foreach (var param in query.ExtraParams)
            {
                var key = param.Key;
                var value = param.Value;
                Filters.Add(x => EF.Property<string>(x!, key) == value);
            }
        }
    }

    public IQueryable<T> ApplyFilters(IQueryable<T> query)
    {
        foreach (var filter in Filters)
            query = query.Where(filter);

        return query;
    } 

    public IQueryable<T> AddPagination(IQueryable<T> query)
    {
        query = query.Skip(Skip).Take(Take);

        return query;
    }

    public async Task<BaseQueryResponse<IEnumerable<T>>> ExecuteQueryAsync(IQueryable<T> query, bool? addFilters = true, bool? addPagination = true)
    {
        if (addFilters == true)
        {
            query = ApplyFilters(query);
        }

        int totalRows = 0;
        int totalPages = 0;

        if (addPagination == true)
        {
            totalRows = await query.CountAsync();
            totalPages = (int)Math.Ceiling((double)totalRows / (double)Take);
            query = AddPagination(query);
        }

        return new BaseQueryResponse<IEnumerable<T>>
        {
            Data = await query.ToListAsync(),
            TotalRows = totalRows,
            TotalPages = totalPages
        };
    }

    private void GetFieldAndConditionFromExtraParamKey(string extraParamKey, out string attributeName, out string condition)
    {
        var conditionParts = extraParamKey.Split("__");
        if (conditionParts.Length > 0)
        {
            attributeName = conditionParts[0];
            condition = conditionParts[1].ToLower();
        }
        else
        {
            attributeName = extraParamKey;
            condition = "eq";
        }

        // aqui procurar se o att está na classe
        // também validar se é um condition valida
    }
}