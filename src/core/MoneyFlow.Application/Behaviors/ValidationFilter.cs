using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MoneyFlow.Application.Behaviors;

public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Obtém os argumentos da action (os comandos)
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null)
            {
                continue;
            }

            // Obtém o validador para o tipo do argumento
            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator is not null)
            {
                var validationContext = new ValidationContext<object>(argument);
                var validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    context.Result = new BadRequestObjectResult(new
                    {
                        Erros = validationResult.Errors.Select(e => new { ErrorCode = e.ErrorCode, ErrorMessage = $"{e.PropertyName}: {e.ErrorMessage}" })
                    });
                    return;
                }
            }
        }

        // Continua a execução da action se a validação passar
        await next();
    }
}