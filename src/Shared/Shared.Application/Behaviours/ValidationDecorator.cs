using FluentValidation;
using FluentValidation.Results;
using Shared.Application.Messaging;
using Shared.Domain;

namespace Shared.Application.Behaviours;

internal static class ValidationDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
         : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
                return await innerHandler.HandleAsync(command, cancellationToken);

            return Result.Failure<TResponse>(CreateValidationError(validationFailures));
        }
    }

    internal sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        IEnumerable<IValidator<TCommand>> validators)
         : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(command, validators);

            if (validationFailures.Length == 0)
                return await innerHandler.HandleAsync(command, cancellationToken);

            return Result.Failure(CreateValidationError(validationFailures));
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        ICommandHandler<TQuery, TResponse> innerHandler,
        IEnumerable<IValidator<TQuery>> validators)
         : ICommandHandler<TQuery, TResponse>
        where TQuery : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            ValidationFailure[] validationFailures = await ValidateAsync(query, validators);

            if (validationFailures.Length == 0)
                return await innerHandler.HandleAsync(query, cancellationToken);

            return Result.Failure<TResponse>(CreateValidationError(validationFailures));
        }
    }

    private static async Task<ValidationFailure[]> ValidateAsync<TCommand>(
        TCommand command,
        IEnumerable<IValidator<TCommand>> validators)
    {
        if (!validators.Any())
            return [];

        var context = new ValidationContext<TCommand>(command);

        ValidationResult[] validationResults = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    }

    private static List<Error> CreateValidationError(ValidationFailure[] validationFailures) =>
        validationFailures.Select(f => Error.ValidationError(f.ErrorMessage)).ToList();

}
