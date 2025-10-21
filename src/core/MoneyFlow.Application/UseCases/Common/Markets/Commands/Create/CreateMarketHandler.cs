using Mediator.Abstractions;
using MoneyFlow.Common.Communications;
using MoneyFlow.Common.Exceptions;

namespace MoneyFlow.Application.UseCases.Common.Markets.Commands.Create;

public class CreateMarketHandler : IHandler<CreateMarketCommand, BaseResponse<string>>
{
    public async Task<BaseResponse<string>> HandleAsync(CreateMarketCommand request, CancellationToken cancellationToken = default)
    {
        throw new ErrorOnValidationException("", "Not implemented yet");
    }
}
