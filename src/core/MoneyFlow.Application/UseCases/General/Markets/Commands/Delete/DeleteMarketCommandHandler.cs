using SharedKernel.Communications;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands.Delete;

public class DeleteMarketCommandHandler : IHandler<DeleteMarketCommand, BaseResponse<string>>
{
    public Task<BaseResponse<string>> HandleAsync(DeleteMarketCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
