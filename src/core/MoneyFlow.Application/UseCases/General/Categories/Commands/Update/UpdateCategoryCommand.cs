using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Update;

public sealed record UpdateCategoryCommand(
    Guid? ExternalId, 
    string? Name, 
    bool? Active) : IRequest<BaseResponse<string>>;