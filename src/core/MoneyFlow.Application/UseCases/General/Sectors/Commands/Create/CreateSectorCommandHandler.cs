using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;

internal class CreateSectorCommandHandler(
    ICategoryReadRepository categoryReadRepository,
    ISectorWriteRepository sectorWriteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateSectorCommand, Guid>
{
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;
    private readonly ISectorWriteRepository _sectorWriteRepository = sectorWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(CreateSectorCommand request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryReadRepository.GetByExternalIdAsync(request.CategoryExternalId, cancellationToken);
        if (category is null)
            return Result.Failure<Guid>(Error.RecordNotFound("Category not found."));

        var sector = Sector.Create(request.Name, category);

        if (sector.IsFailure)
            return Result.Failure<Guid>(sector.Errors!);

        await _sectorWriteRepository.CreateAsync(sector.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(sector.Value.ExternalId!.Value);
    }
}
