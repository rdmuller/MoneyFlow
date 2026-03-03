using Mediator.Abstractions;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;

internal class UpdateCurrencyCommandHandler(
    IUnitOfWork unitOfWork, ICurrencyWriteRepository currencyWriteRepository)
    : IHandler<UpdateCurrencyCommand, BaseResponse<string>>
{
    private readonly ICurrencyWriteRepository _currencyWriteRepository = currencyWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(UpdateCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.ExternalId.HasValue)
            throw ErrorOnValidationException.RequiredFieldIsEmpty("Currency is required");

        var currency = await _currencyWriteRepository.GetByExternalIdAsync(request.ExternalId.Value, cancellationToken);
        if (currency is null)
            throw DataBaseException.RecordNotFound("Currency not found");

        currency.Update(request.Name!, request.Symbol!, request.Active);

        _currencyWriteRepository.Update(currency, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }
}
