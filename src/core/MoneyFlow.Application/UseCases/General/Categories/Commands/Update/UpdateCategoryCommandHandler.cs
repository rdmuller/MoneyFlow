using Mediator.Abstractions;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Update;

internal class UpdateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, IUnitOfWork unitOfWork) : IHandler<UpdateCategoryCommand, BaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;

    public async Task<BaseResponse<string>> HandleAsync(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            throw ErrorOnValidationException.RequiredFieldIsEmpty("Category is required");

        var category = await _categoryWriteRepository.GetByExternalIdAsync((Guid)request.ExternalId, cancellationToken);
        if (category is null)
            throw DataBaseException.RecordNotFound("Category not found");

        category.Update(request.Name!, request.Active);

        _categoryWriteRepository.Update(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }
}
