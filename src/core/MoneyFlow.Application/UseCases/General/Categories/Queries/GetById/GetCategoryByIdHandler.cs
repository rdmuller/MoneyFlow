using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using MoneyFlow.Domain.General.Categories;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Categories.Queries.GetById;
internal class GetCategoryByIdHandler(ICategoryReadRepository categoryReadRepository) : IHandler<GetCategoryByIdQuery, BaseResponse<CategoryQueryDTO>>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;

    public async Task<BaseResponse<CategoryQueryDTO>> HandleAsync(GetCategoryByIdQuery request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryReadRepository.GetByIdAsync(request.Id, cancellationToken);

        if (category is null)
            throw new NoContentException();

        return new BaseResponse<CategoryQueryDTO>
        {
            Data = category.Adapt<CategoryQueryDTO>()
        };
    }
}
