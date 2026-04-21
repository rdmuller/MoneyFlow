using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Delete;

internal class DeleteCategoryCommandHandler(
    ICategoryWriteRepository categoryWriteRepository,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(DeleteCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryWriteRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);
        if (category is null)
            return Result.Failure(Error.RecordNotFound("Category not found."));

        _categoryWriteRepository.Delete(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
