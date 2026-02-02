using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetByExternalId;

internal class GetCategoryByExternalIdQueryHandler(ICategoryReadRepository categoryReadRepository) : IHandler<GetSectorByExternalIdQuery, BaseResponse<CategoryQueryDTO>>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;

    public async Task<BaseResponse<CategoryQueryDTO>> HandleAsync(GetSectorByExternalIdQuery request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryReadRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);

        if (category is null)
            throw new NoContentException();

        return new BaseResponse<CategoryQueryDTO>
        {
            Data = category.Adapt<CategoryQueryDTO>()
        };
    }
}
