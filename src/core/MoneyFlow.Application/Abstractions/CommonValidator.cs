using FluentValidation;
using SharedKernel.Abstractions;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.Abstractions;

public abstract class CommonValidator<T> : AbstractValidator<T>
{
    public async Task<List<Error>> ValidateWithErrorsAsync(T context, CancellationToken cancellation = default)
    {
        var result = await base.ValidateAsync(context, cancellation);

        if (result.IsValid)
            return new List<Error>();

        return result.Errors
            .Select(e => new Error(e.ErrorCode, e.ErrorMessage))
            .ToList();
    }

    public async Task ValidateAndThrowWhenErrorAsync(T context, CancellationToken cancellationToken = default)
    {
        var errors = await ValidateWithErrorsAsync(context, cancellationToken);
        if (errors.Count > 0)
            throw new ErrorOnValidationException(errors);
    }
}
