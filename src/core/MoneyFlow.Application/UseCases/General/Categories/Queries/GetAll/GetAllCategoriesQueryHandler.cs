using Mapster;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Abstractions;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;

internal class GetAllCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
    : IQueryHandler<GetAllCategoriesQuery, BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;

    public async Task<Result<BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>> HandleAsync(GetAllCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>.Create(categories.Adapt<BaseQueryResponse<IReadOnlyList<CategoryQueryDTO>>>());
    }
}
