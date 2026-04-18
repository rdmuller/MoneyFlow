using Mapster;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;

internal class GetCategoryByExternalIdQueryHandler(ICategoryReadRepository categoryReadRepository) :
    IQueryHandler<GetCategoryByExternalIdQuery, CategoryQueryDTO>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;

    public async Task<Result<CategoryQueryDTO>> HandleAsync(GetCategoryByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        return Result<CategoryQueryDTO>.Create(category.Adapt<CategoryQueryDTO>());
    }
}
