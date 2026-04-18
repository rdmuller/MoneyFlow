using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;

internal class UpdateSectorCommandHandler(
    ISectorWriteRepository sectorWriteRepository,
    ICategoryReadRepository categoryReadRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateSectorCommand>
{
    private readonly ISectorWriteRepository _sectorWriteRepository = sectorWriteRepository;
    private readonly ICategoryReadRepository _categoryReadRepository = categoryReadRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public async Task<Result> HandleAsync(UpdateSectorCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            return Result.Failure(Error.RequiredFieldIsEmpty("Sector is required"));

        var sector = await _sectorWriteRepository.GetByExternalIdAsync(request.ExternalId.Value);
        if (sector is null)
            return Result.Failure(Error.RecordNotFound("Sector not found"));

        var category = await _categoryReadRepository.GetByExternalIdAsync(request.CategoryId ?? Guid.Empty, cancellationToken);
        if (category is null)
            return Result.Failure(Error.RecordNotFound("Category not found"));

        var result = sector.Update(request.Name!, category!, (bool)request.Active!);
        if (result.IsFailure)
            return Result.Failure(result.Errors!);

        _sectorWriteRepository.Update(sector);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
