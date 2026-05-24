using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Currencies;
using Shared.Application.Messaging;
using Shared.Domain;

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
        Currency? currency = await _currencyWriteRepository.GetByExternalIdAsync(request.ExternalId, cancellationToken);
        if (currency is null)
            return Result.Failure(Error.RecordNotFound("Currency not found."));

        _currencyWriteRepository.Delete(currency, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
