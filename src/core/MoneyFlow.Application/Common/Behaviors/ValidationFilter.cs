using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedKernel.Abstractions;
using SharedKernel.Exceptions;

namespace MoneyFlow.Application.Common.Behaviors;

public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Obtém os argumentos da action (os comandos)
        foreach (object? argument in context.ActionArguments.Values)
        {
            if (argument == null)
            {
                continue;
            }

            // Obtém o validador para o tipo do argumento
            Type validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator is not null)
            {
                ValidationContext<object> validationContext = new(argument);
                FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    var failures = validationResult
                        .Errors
                        .Select(e => new Error(e.ErrorCode, e.ErrorMessage))
                        .ToList();

                    throw new ErrorOnValidationException(failures);
                }
            }
        }

        // Continua a execução da action se a validação passar
        await next();
    }
}