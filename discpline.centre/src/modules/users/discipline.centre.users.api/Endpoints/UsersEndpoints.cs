using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Users.Commands;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.application.Users.DTOs.Endpoints;
using discipline.centre.users.application.Users.Queries;
using discipline.centre.users.application.Users.Services;
using discipline.centre.users.domain.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SignUpDto = discipline.centre.users.application.Users.DTOs.Endpoints.SignUpDto;

// ReSharper disable All
namespace discipline.centre.users.api.Endpoints;

internal static class UsersEndpoints
{
    private const string UserTag = "users";
    private const string GetById = "GetById";
    
    internal static WebApplication MapUsersEndpoints(this WebApplication app)
    {
        app.MapPost($"api/{UserTag}", async (SignUpDto dto,
            CancellationToken cancellationToken, ICqrsDispatcher commandDispatcher, IHttpContextAccessor contextAccessor) =>
            {
                var userId = UserId.New();
                await commandDispatcher.HandleAsync(dto.MapAsCommand(userId), cancellationToken);
                contextAccessor.AddResourceIdHeader(userId.ToString());
                
                return Results.CreatedAtRoute(nameof(GetById),  new {userId = userId.ToString()}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("SignUp")
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-up user"
            });
        
        app.MapPost($"api/{UserTag}/tokens", async (SignInCommand command,
                ICqrsDispatcher commandDispatcher, ITokenStorage tokenStorage, CancellationToken cancellationToken) =>
            {
                await commandDispatcher.HandleAsync(command, cancellationToken);
                var jwt = tokenStorage.Get(); 
                
                return Results.Ok(jwt);
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("SignIn")
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-in user"
            });
        
        app.MapPost($"api/{UserTag}/refresh-tokens", async (RefreshRequestDto dto,
                ICqrsDispatcher commandDispatcher, ITokenStorage tokenStorage, CancellationToken cancellationToken) =>
            {
                // TODO: Mapper
                var command = new RefreshTokenCommand(dto.RefreshToken, UserId.Parse(dto.UserId));
                
                await commandDispatcher.HandleAsync(command, cancellationToken);
                var jwt = tokenStorage.Get(); 
                
                return Results.Ok(jwt);
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("Refresh")
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Refreshed token for user"
            });
        
        app.MapPost($"api/{UserTag}/subscription-order", async (CreateUserSubscriptionOrderDto dto,
                IIdentityContext identityContext, ICqrsDispatcher commandDispatcher, CancellationToken cancellationToken) =>
            {
                var subscriptionOrderId = SubscriptionOrderId.New();
                var userId = identityContext.GetUser();

                if (userId is null)
                {
                    return Results.Unauthorized();
                }

                await Task.Delay(1);
                
                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("CreateUserSubscriptionOrder")
            .WithTags(UserTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds subscription order for user"
            })
            .RequireAuthorization();
        
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