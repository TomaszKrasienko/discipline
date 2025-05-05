using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure;

public abstract class BaseFacade<T>(ILogger<T> logger) where T : class
{
    public async Task<OneOf<bool, string>> ValidateResponse(HttpResponseMessage response,
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
}