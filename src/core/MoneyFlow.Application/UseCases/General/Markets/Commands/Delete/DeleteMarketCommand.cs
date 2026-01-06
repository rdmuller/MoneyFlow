using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;

public sealed record DeleteMarketCommand(long Id) : IRequest<BaseResponse<string>>;