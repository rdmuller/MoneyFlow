using FluentValidation;
using MoneyFlow.Application.Abstractions;
using MoneyFlow.Application.DTOs.General.Markets;

namespace MoneyFlow.Application.UseCases.General.Markets.Commands;

public class MarketCommandDTOValidator : CommonValidator<MarketCommandDTO>
{
    public MarketCommandDTOValidator()
    {
        RuleFor(x => x).NotNull().WithMessage("Market 'data' must be provided.");
        When(x => x is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Market name is required")
                .MaximumLength(100).WithMessage("Market name must not exceed 100 characters");
        });
    }
}
