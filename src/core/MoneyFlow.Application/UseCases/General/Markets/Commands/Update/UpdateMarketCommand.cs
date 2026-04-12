using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Update;

public sealed record UpdateMarketCommand(
        Guid? ExternalId,
        string? Name,
        bool? Active) : IRequest<BaseResponse<string>>;