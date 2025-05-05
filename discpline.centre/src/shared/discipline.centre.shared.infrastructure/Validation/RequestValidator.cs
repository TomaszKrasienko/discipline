using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace discipline.centre.shared.infrastructure.Validation;

public sealed class RequestValidator<TRequest>(IServiceProvider serviceProvider) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.GetArgument<TRequest>(0);
        
        var scope = serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetService<IValidator<TRequest>>();

        if (validator is null)
        {
            return await next(context); 
        }
        
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors
                .ToDictionary(x => x.PropertyName, y=> y.ErrorMessage));
        }
            
        return await next(context);
    }
}