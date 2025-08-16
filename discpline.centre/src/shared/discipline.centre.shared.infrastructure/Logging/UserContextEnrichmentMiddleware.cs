using System.Diagnostics;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace discipline.centre.shared.infrastructure.Logging;

internal sealed class UserContextEnrichmentMiddleware(
    IServiceProvider serviceProvider,
    ILogger<UserContextEnrichmentMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using var scope = serviceProvider.CreateScope();
        var identityContext = scope.ServiceProvider.GetService<IIdentityContext>();

        if (identityContext is null)
        {
            await next(context);
            return;
        }
        
        if (identityContext.IsAuthenticated)
        {
            var accountId = identityContext.GetAccount();

            if (accountId is null)
            {
                await next(context);
                return;    
            }
            
            Activity.Current?.SetTag("account.id", accountId.ToString());

            var data = new Dictionary<string, object>()
            {
                ["AccountId"] = accountId.ToString()!
            };

            using (logger.BeginScope(data))
            {
                await next(context);
                return;
            }
        }
        
        await next(context);
    }
}