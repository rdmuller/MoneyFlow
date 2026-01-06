using Mediator.Abstractions;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Markets;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Update;

internal class UpdateMarketCommandHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork) : IHandler<UpdateMarketCommand, BaseResponse<string>>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(UpdateMarketCommand request, CancellationToken cancellationToken = default)
    {
        if (request.ExternalId.HasValue == false)
            throw ErrorOnValidationException.RequiredFieldIsEmpty("Market id is required");

        var market = await _marketWriteRepository.GetByExternalIdAsync((Guid)request.ExternalId, cancellationToken);
        if (market is null)
            throw DataBaseException.RecordNotFound("Market not found");

        market.Update(request.Name!, request.Active);

        _marketWriteRepository.Update(market, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }
}
