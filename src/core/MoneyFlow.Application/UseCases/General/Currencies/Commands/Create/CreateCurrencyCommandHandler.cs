using Mediator.Abstractions;
using MoneyFlow.Domain.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;

internal class CreateCurrencyCommandHandler(IUnitOfWork unitOfWork) : IHandler<CreateCurrencyCommand, BaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(CreateCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        var currency = Domain.General.Entities.Currencies.Currency.Create(request.name);


        await _unitOfWork.CommitAsync(cancellationToken);

        return BaseResponse<string>.CreateNewObjectIdResponse(currency.ExternalId);
    }
}
