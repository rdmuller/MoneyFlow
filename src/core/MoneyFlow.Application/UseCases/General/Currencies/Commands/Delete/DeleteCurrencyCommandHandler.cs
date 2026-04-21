using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Delete;

internal class DeleteCurrencyCommandHandler(
    ICurrencyWriteRepository currencyWriteRepository,
    IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteCurrencyCommand>
{
    private readonly ICurrencyWriteRepository _currencyWriteRepository = currencyWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(DeleteCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        var currency = await _currencyWriteRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);
        if (currency is null)
            return Result.Failure(Error.RecordNotFound("Currency not found."));

        _currencyWriteRepository.Delete(currency, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
