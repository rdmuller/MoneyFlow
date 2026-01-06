using Microsoft.EntityFrameworkCore;
using SharedKernel.Communications;
using SharedKernel.Exceptions;
using System.Linq.Expressions;
using static MoneyFlow.Infra.Helpers.AttributePropertiesCache;


namespace MoneyFlow.Infra.DataAccess.Extensions;

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
            AddExtraParamsFilters(query.ExtraParams);
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

    public async Task<BaseQueryResponse<IEnumerable<T>>> ExecuteQueryAsync(
        IQueryable<T> query, 
        bool addFilters = true, 
        bool addPagination = true, 
        Expression<Func<T, T>>? selectorFields = null,
        Expression<Func<T, object>>? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        if (addFilters == true)
        {
            query = ApplyFilters(query);
        }

        int totalRows = 0;
        int totalPages = 0;

        if (addPagination)
        {
            totalRows = await query.CountAsync(cancellationToken);
            totalPages = (int)Math.Ceiling(totalRows / (double)Take);
        }

        IEnumerable<T> resultQuery = [];

        if (totalRows > 0 || !addPagination)
        {
            if (orderBy is not null)
                query = query.OrderBy(orderBy);

            query = AddPagination(query);

            if (selectorFields is null)
                resultQuery = await query.ToListAsync(cancellationToken);
            else
                resultQuery = await query.Select(selectorFields).ToListAsync(cancellationToken);
        }

        return new BaseQueryResponse<IEnumerable<T>>
        {
            Data = resultQuery,
            TotalRows = totalRows,
            TotalPages = totalPages
        };
    }

    private void AddExtraParamsFilters(Dictionary<string, string> extraParams)
    {
        foreach (var param in extraParams)
        {
            if (param.Value is not null)
            {
                AttributeProperties attributeProperties;
                string condition;

                GetFieldAndConditionFromExtraParamKey(param.Key, out attributeProperties, out condition);
                if (attributeProperties is null)
                    continue;

                if (attributeProperties.Type.Contains("string"))
                    AddStringExtraParamFilter(attributeProperties.RealName, condition, param.Value.ToLower());

                else if (attributeProperties.Type.Contains("int") || attributeProperties.Type.Contains("short") || attributeProperties.Type.Contains("long"))
                    AddNumericWihtoutDecimalsExtraParamFilter(attributeProperties.RealName, condition, param.Value);

                else throw new NotImplementedException($"The filter for the attribute type '{attributeProperties.Type}' is not implemented.");
            }
        }
    }

    private void AddStringExtraParamFilter(string attributeName, string condition, string value)
    {
        switch (condition)
        {
            case "eq":
                Filters.Add(x => EF.Property<string>(x!, attributeName).ToLower().Equals(value));
                break;
            case "neq":
                Filters.Add(x => !EF.Property<string>(x!, attributeName).ToLower().Equals(value));
                break;
            case "like":
                Filters.Add(x => EF.Property<string>(x!, attributeName).ToLower().Contains(value));
                break;
            default:
                throw new DataBaseException("InvalidFilterCondition", $"Invalid condition ({condition}) for string attribute filter.");
        }
    }

    private void AddNumericWihtoutDecimalsExtraParamFilter(string attributeName, string condition, string stringValue)
    {
        long value;

        if (long.TryParse(stringValue, out value))
        {
            switch (condition)
            {
                case "gt":
                    Filters.Add(x => EF.Property<long>(x!, attributeName) > value);
                    break;
                case "gte":
                    Filters.Add(x => EF.Property<long>(x!, attributeName) >= value);
                    break;
                case "lt":
                    Filters.Add(x => EF.Property<long>(x!, attributeName) < value);
                    break;
                case "lte":
                    Filters.Add(x => EF.Property<long>(x!, attributeName) <= value);
                    break;
                case "neq":
                    Filters.Add(x => !EF.Property<long>(x!, attributeName).Equals(value));
                    break;
                case "eq":
                    Filters.Add(x => EF.Property<long>(x!, attributeName).Equals(value));
                    break;
                default:
                    throw new DataBaseException("InvalidFilterCondition", $"Invalid condition ({condition}) for numeric attribute filter.");
            }
        }
        else
            throw new DataBaseException("InvalidFilterValue", $"The value '{stringValue}' is not valid for numeric attribute filter.");
    }

    private static void GetFieldAndConditionFromExtraParamKey(string extraParamKey, out AttributeProperties? attributeProperties, out string condition)
    {
        var conditionParts = extraParamKey.Split("__");
        string attributeName;

        if (conditionParts.Length > 1)
        {
            attributeName = conditionParts[0];
            condition = conditionParts[1].ToLower();

            if (string.IsNullOrWhiteSpace(condition))
                condition = "eq";
        }
        else
        {
            attributeName = extraParamKey;
            condition = "eq";
        }

        attributeProperties = !string.IsNullOrWhiteSpace(attributeName) ? GetAttributeProperties(typeof(T), attributeName) : null!;
    }
}