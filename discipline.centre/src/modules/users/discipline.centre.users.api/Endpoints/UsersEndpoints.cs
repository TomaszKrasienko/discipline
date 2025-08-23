using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.DTOs.Responses;
using discipline.centre.users.application.Users.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable All
namespace Microsoft.AspNetCore.Builder;

internal static class UsersEndpoints
{
    private const string UserTag = "users";
    
    internal static WebApplication MapUsersEndpoints(this WebApplication app)
    {
        app.MapGet(
            $"api/{UserTag}/me", 
            async (
                ICqrsDispatcher dispatcher,
                IIdentityContext identityContext,
                CancellationToken cancellationToken) =>
        {
            var accountId = identityContext.GetAccount();

            if (accountId is null)
            {
                return Results.Unauthorized();
            }
            
            var user = await dispatcher.SendAsync(new GetUserByAccountIdQuery(accountId.Value), cancellationToken);
            return user is null ? Results.NotFound() : Results.Ok(user);
        })
        .Produces(StatusCodes.Status200OK, typeof(UserResponseDto))
        .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .WithName("GetAuthorizedUser")
        .WithTags(UserTag)
        .WithOpenApi(operation => new (operation)
        {
            Description = "Gets authorized user"
        })
        .RequireAuthorization();
            
        return app;
    }
}