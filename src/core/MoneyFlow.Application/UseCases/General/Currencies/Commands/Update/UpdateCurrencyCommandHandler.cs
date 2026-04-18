using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;

internal class UpdateCurrencyCommandHandler(
    IUnitOfWork unitOfWork, ICurrencyWriteRepository currencyWriteRepository)
    : ICommandHandler<UpdateCurrencyCommand>
{
    private readonly ICurrencyWriteRepository _currencyWriteRepository = currencyWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(UpdateCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            return Result.Failure(Error.RequiredFieldIsEmpty("Currency is required"));

        var currency = await _currencyWriteRepository.GetByExternalIdAsync(request.ExternalId.Value, cancellationToken);
        if (currency is null)
            return Result.Failure(Error.RecordNotFound("Currency not found"));

        var result = currency.Update(request.Name!, request.Symbol!, request.Active);
        if (result.IsFailure)
            return Result.Failure(result.Errors!);

        _currencyWriteRepository.Update(currency, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
