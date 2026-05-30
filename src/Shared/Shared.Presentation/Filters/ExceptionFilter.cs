using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Application.Exceptions;
using Shared.Domain;

namespace Shared.Presentation.Filters;

/// <summary>
/// Global exception filter for handling application-specific exceptions.
/// Following Clean Architecture: Filters belong to Presentation layer, not Application.
/// </summary>
public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ErrorOnValidationException:
                HandleErrorOnValidationException(context);
                break;

            case AuthorizationException:
                HandleAuthorizationException(context);
                break;

            case DataBaseException:
                HandleDataBaseException(context);
                break;

            case NoContentException:
                HandleNoContentException(context);
                break;

            default:
                // For unknown exceptions, let the default behavior handle it
                // or log and return a generic 500 error
                HandleUnknownException(context);
                break;
        }
    }

    private static void HandleAuthorizationException(ExceptionContext context)
    {
        var exception = (AuthorizationException)context.Exception;
        object response = CreateErrorResponseFromBaseErrors(exception.Errors);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(response);
        context.ExceptionHandled = true;
    }

    private static void HandleErrorOnValidationException(ExceptionContext context)
    {
        var exception = (ErrorOnValidationException)context.Exception;
        object response = CreateErrorResponseFromBaseErrors(exception.Errors);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(response);
        context.ExceptionHandled = true;
    }

    private static void HandleDataBaseException(ExceptionContext context)
    {
        var exception = (DataBaseException)context.Exception;
        object response = CreateErrorResponseFromBaseErrors(exception.Errors);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(response);
        context.ExceptionHandled = true;
    }

    private static void HandleNoContentException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
        context.Result = new NoContentResult();
        context.ExceptionHandled = true;
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        Error error = new("INTERNAL_SERVER_ERROR", "An unexpected error occurred.");
        object response = CreateErrorResponse([error]);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(response);
        context.ExceptionHandled = true;
    }

    private static object CreateErrorResponseFromBaseErrors(IEnumerable<BaseError> baseErrors)
    {
        return new
        {
            Success = false,
            Errors = baseErrors.Select(e => new
            {
                e.ErrorCode,
                e.ErrorMessage
            })
        };
    }

    private static object CreateErrorResponse(IReadOnlyList<Error> errors)
    {
        return new
        {
            Success = false,
            Errors = errors.Select(e => new
            {
                e.Code,
                e.Message
            })
        };
    }
}
