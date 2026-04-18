using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Create;

internal class CreateMarketCommandHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateMarketCommand, Guid>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(CreateMarketCommand request, CancellationToken cancellationToken = default)
    {
        var market = Market.Create(request.Name ?? "");

        if (market.IsFailure)
            return Result.Failure<Guid>(market.Errors!);

        await _marketWriteRepository.CreateAsync(market.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(market.Value.ExternalId!.Value);
    }
}
