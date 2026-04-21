using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Sectors;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Delete;

internal class DeleteSectorCommandHandler(
    ISectorWriteRepository sectorWriteRepository,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteSectorCommand>
{
    private readonly ISectorWriteRepository _sectorWriteRepository = sectorWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(DeleteSectorCommand request, CancellationToken cancellationToken = default)
    {
        var sector = await _sectorWriteRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);
        if (sector is null)
            return Result.Failure(Error.RecordNotFound("Sector not found."));

        _sectorWriteRepository.Delete(sector, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
