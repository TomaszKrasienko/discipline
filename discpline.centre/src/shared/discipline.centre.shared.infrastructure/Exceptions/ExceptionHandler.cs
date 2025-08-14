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
        await HandleException(exception, httpContext, cancellationToken);
        
        logger.LogError(exception, exception.Message);
        return true;
    }

    private async Task HandleException(Exception exception,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        int status;
        ProblemDetails problemDetails;
        
        if (exception is DisciplineException disciplineException &&
            IsAuthorizeException(disciplineException.Code))
        {
            var errorMessage = errorLocalizationService.GetMessage(disciplineException.Code);
            
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "SignIn.Unauthorized",
                Type = nameof(DisciplineException),
                Detail = errorMessage
            };
            
            status = StatusCodes.Status400BadRequest;
        }
        else
        {
            var (statusCode, code, args) = exception switch
            {
                NotFoundException exc => (StatusCodes.Status404NotFound, exc.Code, exc.Args),
                DisciplineException exc => (StatusCodes.Status400BadRequest, exc.Code, exc.Args),
                _ => (StatusCodes.Status500InternalServerError, "Unexpected", null)
            };
        
            var errorMessage = errorLocalizationService.GetMessage(code);
            problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = code,
                Type = exception.GetType().Name,
                Detail = args is null ? errorMessage : String.Format(errorMessage, args)
            };
        
            status = statusCode;
        }
        
        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    private bool IsAuthorizeException(string code)
        => code.StartsWith("SignIn");
}