using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Validation.Mappers;
using discipline.centre.users.application.Accounts.DTOs;
using discipline.centre.users.application.Accounts.DTOs.Requests;
using discipline.centre.users.application.Accounts.Services;
using discipline.libs.cqrs.Abstractions;
using FluentValidation;
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
                ICqrsDispatcher dispatcher,
                IValidator<SignUpRequestDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return Results.UnprocessableEntity(validationResult.ToProblemDetails());
                }
                
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
                ITokenStorage tokenStorage,
                IValidator<SignInRequestDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                
                if (!validationResult.IsValid)
                {
                    return Results.UnprocessableEntity(validationResult.ToProblemDetails());
                }
                
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
        
        app.MapPost(
                $"api/{AccountTag}/refreshed",
                async (
                    RefreshRequestDto request,
                    CancellationToken cancellationToken,
                    ICqrsDispatcher dispatcher,
                    ITokenStorage tokenStorage,
                    IValidator<RefreshRequestDto> validator) =>
                {
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);
                
                    if (!validationResult.IsValid)
                    {
                        return Results.UnprocessableEntity(validationResult.ToProblemDetails());
                    }
                
                    await dispatcher.HandleAsync(
                        request.ToCommand(),
                        cancellationToken);
                
                    var tokens = tokenStorage.Get();
                    return Results.Ok(tokens);
                })
            .Produces(StatusCodes.Status200OK, typeof(TokensDto))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("Refresh")
            .WithTags(AccountTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Refresh token user"
            });
        
        //TODO: Subscription order

        return app;
    }
}