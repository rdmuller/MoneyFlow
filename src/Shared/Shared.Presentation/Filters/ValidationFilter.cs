using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Application.Exceptions;
using Shared.Domain;

namespace Shared.Presentation.Filters;

/// <summary>
/// Global validation filter using FluentValidation.
/// Following Clean Architecture: Filters belong to Presentation layer, not Application.
/// </summary>
public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Validates all action arguments using FluentValidation
        foreach (object? argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            // Gets the validator for the argument type
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

        // Continues action execution if validation passes
        await next();
    }
}
