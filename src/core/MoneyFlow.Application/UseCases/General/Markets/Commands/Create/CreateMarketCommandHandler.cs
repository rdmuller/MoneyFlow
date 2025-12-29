using Mediator.Abstractions;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Repositories;
using MoneyFlow.Domain.General.Repositories.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Create;

internal class CreateMarketCommandHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork) : IHandler<CreateMarketCommand, BaseResponse<string>>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(CreateMarketCommand request, CancellationToken cancellationToken = default)
    {
        var market = Market.Create(request.Market!.Name!);

        await _marketWriteRepository.CreateAsync(market, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>
        {
            ObjectId = market.Id
        };
    }
}
