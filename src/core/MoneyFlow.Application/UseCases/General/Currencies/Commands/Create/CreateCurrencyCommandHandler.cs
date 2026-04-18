using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Currencies;
using SharedKernel.Abstractions;
using SharedKernel.Mediator;

namespace MoneyFlow.Application.UseCases.General.Currencies.Commands.Create;

internal class CreateCurrencyCommandHandler(IUnitOfWork unitOfWork, ICurrencyWriteRepository currencyWriteRepository) : ICommandHandler<CreateCurrencyCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrencyWriteRepository _currencyWriteRepository = currencyWriteRepository;

    public async Task<Result<Guid>> HandleAsync(CreateCurrencyCommand request, CancellationToken cancellationToken = default)
    {
        var currency = Currency.Create(request.Name, request.Symbol);

        if (currency.IsFailure)
            return Result.Failure<Guid>(currency.Errors!);

        await _currencyWriteRepository.CreateAsync(currency.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result<Guid>.Success(currency.Value.ExternalId!.Value);
    }
}
