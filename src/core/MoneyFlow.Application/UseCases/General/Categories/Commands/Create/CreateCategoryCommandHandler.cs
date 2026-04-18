using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Create;

internal class CreateCategoryCommandHandler(ICategoryWriteRepository categoryWriteRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryWriteRepository _categoryWriteRepository = categoryWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand request, CancellationToken cancellationToken = default)
    {
        var category = Category.Create(request.Name);

        if (category.IsFailure)
            return Result.Failure<Guid>(category.Errors!);

        await _categoryWriteRepository.CreateAsync(category.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(category.Value.ExternalId!.Value);
    }
}
