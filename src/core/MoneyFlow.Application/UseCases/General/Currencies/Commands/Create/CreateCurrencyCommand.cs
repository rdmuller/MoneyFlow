using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;

public sealed record CreateCurrencyCommand(string name) : IRequest<BaseResponse<string>>;
