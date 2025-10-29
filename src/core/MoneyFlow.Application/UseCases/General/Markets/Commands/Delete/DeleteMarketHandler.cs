using Mediator.Abstractions;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;
public class DeleteMarketHandler : IHandler<DeleteMarketCommand, BaseResponse<string>>
{
    public Task<BaseResponse<string>> HandleAsync(DeleteMarketCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
