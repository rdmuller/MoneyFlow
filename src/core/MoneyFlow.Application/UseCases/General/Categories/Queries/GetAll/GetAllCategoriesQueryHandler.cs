/*using Mapster;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Domain.General.Entities.Categories;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetAll;

internal class GetAllCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
    : IQueryHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryQueryDTO>>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;

    public async Task<Result<IReadOnlyList<CategoryQueryDTO>>> HandleAsync(GetAllCategoriesQuery request, CancellationToken cancellationToken = default)
    {
        Result<IEnumerable<Category>> categories = await _categoryReadRepository.GetAllAsync(request.Query, cancellationToken);

        return Result<IReadOnlyList<CategoryQueryDTO>>.Create(categories.Adapt<IReadOnlyList<CategoryQueryDTO>>());
    }
}
*/
