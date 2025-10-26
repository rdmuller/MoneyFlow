using FluentValidation;
using SharedKernel.Exceptions;
using SharedKernel.Communications;

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

    public async Task ValidateAndThrowWhenErrorAsync(T context, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateWithErrorsAsync(context, cancellationToken);
        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}
