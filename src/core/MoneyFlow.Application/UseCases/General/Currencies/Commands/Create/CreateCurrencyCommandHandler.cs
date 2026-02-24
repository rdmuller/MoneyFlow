using Mediator.Abstractions;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;

internal class CreateCurrencyCommandHandler(IUnitOfWork unitOfWork, ICurrencyWriteRepository currencyWriteRepository) : IHandler<CreateCurrencyCommand, BaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrencyWriteRepository _currencyWriteRepository = currencyWriteRepository;

    public async Task<BaseResponse<string>> HandleAsync(CreateCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        var currency = Currency.Create(request.name, request.symbol);

        await _currencyWriteRepository.CreateAsync(currency, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return BaseResponse<string>.CreateNewObjectIdResponse(currency.ExternalId);
    }
}
