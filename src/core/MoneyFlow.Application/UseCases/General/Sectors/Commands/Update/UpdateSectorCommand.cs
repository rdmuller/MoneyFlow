using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Update;

public sealed record UpdateSectorCommand(
    Guid? ExternalId,
    string? Name,
    Guid? CategoryId,
    bool? Active = true) : IRequest<BaseResponse<string>>;
