using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.DTOs.Requests;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.application.Users.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace discipline.centre.users.api.Endpoints;

internal static class AccountEndpoints
{
    private const string AccountTag = "accounts";
    
    internal static WebApplication MapAccountEndpoints(this WebApplication app)
    {
        app.MapPost(
            $"api/{AccountTag}",
            async (
                SignUpRequestDto request,
                CancellationToken cancellationToken,
                ICqrsDispatcher dispatcher) =>
            {
                await dispatcher.HandleAsync(
                    request.ToCommand(AccountId.New()),
                    cancellationToken);
                
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("SignUp")
            .WithTags(AccountTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-up user"
            });
        
        app.MapPost(
            $"api/{AccountTag}/signed-in",
            async (
                SignInRequestDto request,
                CancellationToken cancellationToken,
                ICqrsDispatcher dispatcher,
                ITokenStorage tokenStorage) =>
            {
                await dispatcher.HandleAsync(
                    request.ToCommand(),
                    cancellationToken);
                
                var tokens = tokenStorage.Get();
                return Results.Ok(tokens);
            })
            .Produces(StatusCodes.Status200OK, typeof(TokensDto))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("SignIn")
            .WithTags(AccountTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Signs-in user"
            });
        
        //TODO: Refresh token
        //TODO: Subscription order

        return app;
    }
}