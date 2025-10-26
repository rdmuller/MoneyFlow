using FluentValidation;
using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.DTOs.Common.Markets;
using SharedKernel.Communications;

namespace MoneyFlow.Application.UseCases.Common.Markets.Commands;

public class MarketCommandDTOValidator : CommonValidator<BaseRequest<MarketCommandDTO>>
{
    public MarketCommandDTOValidator()
    {
        RuleFor(x => x.Data).NotNull().WithMessage("Market data must be provided.");
        When(x => x.Data is not null, () =>
        {
            RuleFor(x => x.Data!.Name)
                .NotEmpty().WithMessage("Market name is required")
                .MaximumLength(100).WithMessage("Market name must not exceed 100 characters");
        });
    }
}
