using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetById;

public class GetMarketByIdQuery : IRequest<BaseResponse<MarketQueryDTO>>
{
    public long Id { get; set; }
}
