using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Update;

public class UpdateMarketCommand : IRequest<BaseResponse<string>>
{
    public MarketCommandDTO? Market { get; set; }
}
