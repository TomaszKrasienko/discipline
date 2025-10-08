using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace FluentValidation.Results;

public static class ValidationResultMapperExtensions
{
    public static ProblemDetails ToProblemDetails(this ValidationResult validationResult)
    {
        var failure = validationResult.Errors.Single();

        return new ProblemDetails()
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = failure.ErrorCode,
            Detail = failure.ErrorMessage
        };
    }
}