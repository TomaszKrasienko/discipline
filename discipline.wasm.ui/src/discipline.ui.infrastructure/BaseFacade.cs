using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure;

public abstract class BaseFacade<T>(ILogger<T> logger) where T : class
{
    protected const string UnauthorizedMessage = "user.unauthorized";
    
    protected async Task<OneOf<bool, string>> HandleResponseAsync(HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                logger.LogWarning("User unauthorized");
                return Statics.Unauthorized;
            }
            
            var error = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
            logger.LogError(error?.Title);
            
            return error?.Detail ?? "system.unexpectedError";
        }

        return true;
    }
    
    protected async Task<OneOf<TResult?, string>> HandleResponseAsync<TResult>(HttpResponseMessage response, CancellationToken
        cancellationToken) where TResult : class
    {
        if (response.StatusCode is HttpStatusCode.Unauthorized)
        {
            return UnauthorizedMessage;
        }

        var result = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken);

        return result;
    }
}