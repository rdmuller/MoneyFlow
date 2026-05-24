using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Markets;
using Shared.Application.Messaging;
using Shared.Domain;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Create;

internal class CreateMarketCommandHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateMarketCommand, Guid>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(CreateMarketCommand request, CancellationToken cancellationToken = default)
    {
        Result<Market> market = Market.Create(request.Name ?? "");

        if (market.IsFailure)
            return Result.Failure<Guid>(market.Errors!);

        await _marketWriteRepository.CreateAsync(market.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(market.Value.ExternalId!.Value);
    }
}
