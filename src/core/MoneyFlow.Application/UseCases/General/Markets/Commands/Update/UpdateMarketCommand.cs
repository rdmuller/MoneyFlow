using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Update;

public sealed record UpdateMarketCommand(
        Guid? ExternalId,
        string? Name,
        bool? Active) : IRequest<BaseResponse<string>>;