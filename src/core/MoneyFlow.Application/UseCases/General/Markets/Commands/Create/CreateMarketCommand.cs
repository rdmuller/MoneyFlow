using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Create;

public sealed record CreateMarketCommand(string? Name) : IRequest<BaseResponse<string>>;