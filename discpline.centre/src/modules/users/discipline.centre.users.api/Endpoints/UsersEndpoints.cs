using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable All
namespace discipline.centre.users.api.Endpoints;

internal static class UsersEndpoints
{
    private const string UserTag = "users";
    private const string GetById = "GetById";
    
    internal static WebApplication MapUsersEndpoints(this WebApplication app)
    {
        app.MapGet($"api/{UsersModule.ModuleName}/{UserTag}/{{userId:ulid}}", async (Ulid userId,
                CancellationToken cancellationToken, ICqrsDispatcher dispatcher) =>
            {
                var stronglyUserId = new UserId(userId);
                var result = await dispatcher.SendAsync(new GetUserByIdQuery(stronglyUserId), cancellationToken);
                
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
            .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
            .WithName(GetById)
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Gets user by identifier"
            })
            .RequireAuthorization();
        
        app.MapGet(
            $"api/{UserTag}/me", 
            async (
                ICqrsDispatcher dispatcher,
                IIdentityContext identityContext,
                CancellationToken cancellationToken) =>
        {
            var userId = identityContext.GetUser();

            if (userId is null)
            {
                return Results.Unauthorized();
            }
            
            //TODO: GetUserByAccountId
            var user = await dispatcher.SendAsync(new GetUserByIdQuery(userId.Value), cancellationToken);
            return user is null ? Results.NotFound() : Results.Ok(user);
        })
        .Produces(StatusCodes.Status200OK, typeof(UserDto))
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