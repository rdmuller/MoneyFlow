using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Domain.General.Categories;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;

internal class GetAllCategoriesHandler(ICategoryReadRepository categoryReadRepository) : IHandler<GetAllCategoriesQuery, BaseQueryResponse<IEnumerable<CategoryQueryDTO>>>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;

    public async Task<BaseQueryResponse<IEnumerable<CategoryQueryDTO>>> HandleAsync(GetAllCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        var categories = await _categoryReadRepository.GetAllAsync(request.Query, cancellationToken);

        if (categories.TotalRows == 0)
            throw new NoContentException();

        return categories.Adapt<BaseQueryResponse<IEnumerable<CategoryQueryDTO>>>();
    }
}
