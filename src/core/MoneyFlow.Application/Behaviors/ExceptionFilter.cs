using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoneyFlow.Common.Communications;
using MoneyFlow.Common.Exceptions;
using System.Net;

namespace MoneyFlow.Application.Behaviors;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException)
            HandleErrorOnValidationexception(context);

        else
            throw new NotImplementedException();
    }

    private void HandleErrorOnValidationexception(ExceptionContext context)
    {
        var validationErrors = (ErrorOnValidationException)context.Exception;
        var response = new BaseResponseError(validationErrors.Errors);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(response);
    }
}