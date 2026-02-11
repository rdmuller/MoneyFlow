using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Sectors.Commands.Create;

public sealed record CreateSectorCommand(string Name, Guid categoryExternalId) : IRequest<BaseResponse<string>>;