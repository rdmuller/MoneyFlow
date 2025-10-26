using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Domain.Common.Entities.Markets;
using MoneyFlow.Domain.Common.Repositories;
using MoneyFlow.Domain.Common.Repositories.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.Common.Markets.Commands.Create;

public class CreateMarketHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork) : IHandler<CreateMarketCommand, BaseResponse<string>>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(CreateMarketCommand request, CancellationToken cancellationToken = default)
    {
        var market = request.Market.Adapt<Market>();

        await _marketWriteRepository.CreateMarketAsync(market, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>
        {
            ObjectId = market.Id
        };
    }
}
