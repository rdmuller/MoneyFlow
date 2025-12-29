using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedKernel.Communications;
using SharedKernel.Exceptions;
using System.Net;

namespace MoneyFlow.Application.Common.Behaviors;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException)
            HandleErrorOnValidationException(context);

        else if (context.Exception is AuthorizationException)
            HandleAuthorizationException(context);

        else if (context.Exception is DataBaseException)
            HandleDataBaseException(context);

        else if (context.Exception is NoContentException)
            HandlerNoContentException(context);

        else throw new NotImplementedException();
    }

    private void HandleAuthorizationException(ExceptionContext context)
    {
        var validationErrors = (AuthorizationException)context.Exception;
        var response = new BaseResponseError(validationErrors.Errors);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(response);
    }

    private void HandleErrorOnValidationException(ExceptionContext context)
    {
        var validationErrors = (ErrorOnValidationException)context.Exception;
        var response = new BaseResponseError(validationErrors.Errors);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(response);
    }

    private void HandleDataBaseException(ExceptionContext context)
    {
        var validationErrors = (DataBaseException)context.Exception;
        var response = new BaseResponseError(validationErrors.Errors);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(response);
    }

    private void HandlerNoContentException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
        context.Result = new ObjectResult(null);
    }
}