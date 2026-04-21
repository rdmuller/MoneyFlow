using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid ExternalId) : ICommand;
