using Mediator.Abstractions;
using MoneyFlow.Application.DTOs.General.Categories;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name) : IRequest<BaseResponse<string>>;
