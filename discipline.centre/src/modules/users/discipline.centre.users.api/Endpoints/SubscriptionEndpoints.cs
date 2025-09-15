using discipline.centre.users.application.Subscriptions.DTOs;
using discipline.centre.users.application.Subscriptions.Queries;
using discipline.libs.cqrs.Abstractions;
using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder;

internal static class SubscriptionEndpoints
{
    private const string SubscriptionTag = "subscription";

    internal static WebApplication MapSubscriptionEndpoints(this WebApplication app)
    {
        app.MapGet($"api/{SubscriptionTag}",
                async (
                    ICqrsDispatcher cqrsDispatcher,
                    CancellationToken cancellationToken) =>
                {
                    var result = await cqrsDispatcher.SendAsync(
                        new GetSubscriptionsQuery(),
                        cancellationToken);

                    return Results.Ok(result);
                })
            .Produces(StatusCodes.Status200OK, typeof(List<SubscriptionResponseDto>))
            .WithName("GetSubscriptions")
            .WithTags(SubscriptionTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Gets authorized user"
            });

        return app;
    }
}