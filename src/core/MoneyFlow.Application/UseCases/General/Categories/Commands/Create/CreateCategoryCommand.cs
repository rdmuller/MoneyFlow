using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;
