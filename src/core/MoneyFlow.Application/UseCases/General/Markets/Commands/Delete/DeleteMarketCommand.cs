using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;
public class DeleteMarketCommand : IRequest<BaseResponse<string>>
{
    public long Id { get; set; }
}
