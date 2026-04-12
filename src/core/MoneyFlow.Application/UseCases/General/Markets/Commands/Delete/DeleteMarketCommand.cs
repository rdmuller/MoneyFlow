using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;

public sealed record DeleteMarketCommand(long Id) : IRequest<BaseResponse<string>>;