using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Create;

public sealed record CreateMarketCommand(string? Name) : IRequest<BaseResponse<string>>;