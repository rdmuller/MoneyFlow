﻿using Mapster;
using Mediator.Abstractions;
using MoneyFlow.Domain.General.Repositories;
using MoneyFlow.Domain.General.Repositories.Markets;
using SharedKernel.Communications;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Update;

public class UpdateMarketHandler(
    IMarketWriteRepository marketWriteRepository,
    IUnitOfWork unitOfWork) : IHandler<UpdateMarketCommand, BaseResponse<string>>
{
    private readonly IMarketWriteRepository _marketWriteRepository = marketWriteRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<string>> HandleAsync(UpdateMarketCommand request, CancellationToken cancellationToken = default)
    {
        if (request!.Market!.Id == 0)
            throw new RequiredFieldIsEmptyException("Market id is required");

        var market = await _marketWriteRepository.GetMarketByIdAsync(request.Market.Id, cancellationToken);
        if (market is null)
            throw DataBaseException.RecordNotFound("Market not found");

        request.Market.Adapt(market);

        _marketWriteRepository.UpdateMarket(market, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new BaseResponse<string>();
    }
}
