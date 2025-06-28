using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.Exceptions.Services;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.infrastructure.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Exceptions;

internal sealed class ExceptionHandler(ILogger<IExceptionHandler> logger,
    IErrorLocalizationService errorLocalizationService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            await HandleValidationException(validationException, httpContext, cancellationToken);
            return true;
        }

        await HandleException(exception, httpContext, cancellationToken);
        
        logger.LogError(exception, exception.Message);
        return true;
    }

    private async Task HandleValidationException(ValidationException validationException,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        Dictionary<string, object> errors = [];

        foreach (var arg in validationException.Errors)
        {
            var errorMessage = errorLocalizationService.GetMessage(arg.Value);
            errors.Add(arg.Key, errorMessage);
        }
            
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = validationException.Code,
            Type = validationException.GetType().Name
        };
        
        problemDetails.Extensions.Add("errors", errors);
        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private async Task HandleException(Exception exception,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var (status, code, args) = exception switch
        {
            NotFoundException exc => (StatusCodes.Status404NotFound, exc.Code, exc.Args),
            DisciplineException exc => (StatusCodes.Status400BadRequest, exc.Code, exc.Args),
            _ => (StatusCodes.Status500InternalServerError, "Unexpected", null)
        };
        
        var errorMessage = errorLocalizationService.GetMessage(code);
        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = code,
            Type = exception.GetType().Name,
            Detail = args is null ? errorMessage : String.Format(errorMessage, args)
        };
        
        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}