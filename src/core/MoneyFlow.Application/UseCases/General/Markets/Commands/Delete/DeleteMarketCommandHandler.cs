using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;

internal class DeleteMarketCommandHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteMarketCommand>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(DeleteMarketCommand request, CancellationToken cancellationToken = default)
    {
        var market = await _marketWriteRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);
        if (market is null)
            return Result.Failure(Error.RecordNotFound("Market not found."));

        _marketWriteRepository.Delete(market, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
