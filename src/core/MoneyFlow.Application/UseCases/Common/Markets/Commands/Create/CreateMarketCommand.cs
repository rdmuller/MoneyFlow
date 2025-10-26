using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.Common.Markets.Commands.Create;

public class CreateMarketCommand : IRequest<BaseResponse<string>>
{
    public MarketCommandDTO? Market { get; set; }
}
