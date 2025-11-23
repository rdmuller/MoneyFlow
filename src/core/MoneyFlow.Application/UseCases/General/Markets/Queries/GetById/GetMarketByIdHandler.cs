using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using MoneyFlow.Domain.General.Repositories.Markets;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Markets.Queries.GetById;
internal class GetMarketByIdHandler(IMarketReadRepository marketRepository) : IHandler<GetMarketByIdQuery, BaseResponse<MarketQueryDTO>>
{
    private readonly IMarketReadRepository _marketRepository = marketRepository;

    public async Task<BaseResponse<MarketQueryDTO>> HandleAsync(GetMarketByIdQuery request, CancellationToken cancellationToken = default)
    {
        var market = await _marketRepository.GetByIdAsync(request.Id, cancellationToken);

        if (market is null)
            throw new NoContentException();

        return new BaseResponse<MarketQueryDTO>
        {
            Data = market.Adapt<MarketQueryDTO>()
        };
    }
}
