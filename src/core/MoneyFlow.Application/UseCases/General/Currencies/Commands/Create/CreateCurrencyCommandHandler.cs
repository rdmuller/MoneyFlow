using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;

internal class CreateCurrencyCommandHandler(IUnitOfWork unitOfWork, ICurrencyWriteRepository currencyWriteRepository) : IRequestHandler<CreateCurrencyCommand, BaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrencyWriteRepository _currencyWriteRepository = currencyWriteRepository;

    public async Task<BaseResponse<string>> HandleAsync(CreateCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        var currency = Currency.Create(request.name, request.symbol);

        if (currency.IsFailure)
            return BaseResponse<string>.CreateFailureResponse(currency.Errors!);

        await _currencyWriteRepository.CreateAsync(currency.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return BaseResponse<string>.CreateNewObjectIdResponse(currency.Value.ExternalId);
    }
}
