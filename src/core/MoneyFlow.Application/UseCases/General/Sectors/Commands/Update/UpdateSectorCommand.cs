using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;

public sealed record UpdateSectorCommand(
    Guid? ExternalId,
    string? Name,
    Guid? CategoryId,
    bool? Active = true) : IRequest<BaseResponse<string>>;
