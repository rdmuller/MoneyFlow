using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Markets;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Update;

internal class UpdateMarketCommandHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateMarketCommand>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(UpdateMarketCommand request, CancellationToken cancellationToken = default)
    {
        if (request.ExternalId.HasValue == false)
            return Result.Failure(Error.RequiredFieldIsEmpty("Market id is required"));

        Market? market = await _marketWriteRepository.GetByExternalIdAsync((Guid)request.ExternalId, cancellationToken);
        if (market is null)
            return Result.Failure(Error.RecordNotFound("Market not found"));

        Result result = market.Update(request.Name!, request.Active);
        if (result.IsFailure)
            return Result.Failure(result.Errors!);

        _marketWriteRepository.Update(market, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
