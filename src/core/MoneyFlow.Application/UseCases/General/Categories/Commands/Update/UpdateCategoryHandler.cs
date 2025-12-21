using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Domain.General.Repositories;
using MoneyFlow.Domain.General.Repositories.Categories;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Update;

internal class UpdateCategoryHandler(ICategoryWriteRepository categoryWriteRepository, IUnitOfWork unitOfWork) : IHandler<UpdateCategoryCommand, BaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;

    public async Task<BaseResponse<string>> HandleAsync(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        if (request!.Category!.Id.Equals(0))
            throw new RequiredFieldIsEmptyException("Category Id is required");

        var category = await _categoryWriteRepository.GetByIdAsync(request.Category.Id, cancellationToken);
        if (category is null)
            throw DataBaseException.RecordNotFound("Category not found");

        request.Category.Adapt(category);

        _categoryWriteRepository.Update(category);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }
}
