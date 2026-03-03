using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Update;

public sealed record UpdateCurrencyCommand(
    Guid? ExternalId, 
    string? Name, 
    string? Symbol, 
    bool? Active) : IRequest<BaseResponse<string>>;
