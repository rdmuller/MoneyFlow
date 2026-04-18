using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Update;

internal class UpdateCategoryCommandHandler
    (ICategoryWriteRepository categoryWriteRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;

    public async Task<Result> HandleAsync(UpdateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            return Result.Failure(Error.RequiredFieldIsEmpty("Category is required"));

        var category = await _categoryWriteRepository.GetByExternalIdAsync((Guid)request.ExternalId, cancellationToken);
        if (category is null)
            return Result.Failure(Error.RecordNotFound("Category not found"));

        var result = category.Update(request.Name!, request.Active);
        if (result.IsFailure)
            return Result.Failure(result.Errors!);

        _categoryWriteRepository.Update(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
