using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.UseCases.Common.Markets.Commands.Create;

public class CreateMarketCommand : IRequest<BaseResponse<string>>
{
    public MarketCommandDTO? Market { get; set; }
}
