using FluentValidation;
using MoneyFlow.Common.Communications;

namespace MoneyFlow.Application.Abstractions;

public abstract class CommonValidator<T> : AbstractValidator<T>
{
    public async Task<List<BaseError>> ValidateWithErrorsAsync(T context, CancellationToken cancellation = default) 
    {
        var result = await base.ValidateAsync(context, cancellation);

        if (result.IsValid)
            return new List<BaseError>();

        return result.Errors
            .Select(e => new BaseError() { ErrorCode = e.ErrorCode, ErrorMessage = e.ErrorMessage })
            .ToList();
    }
}
