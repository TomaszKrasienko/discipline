using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Mappers;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Responses.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.Queries;
using discipline.centre.shared.abstractions.CQRS;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Auth;
using discipline.centre.shared.infrastructure.IdentityContext.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using discipline.centre.shared.infrastructure.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable All
namespace Microsoft.AspNetCore.Builder;

internal static class ActivityRulesEndpoints
{
    private const string ActivityRulesTag = "activity-rules";
    private const string GetActivityRuleById = "GetActivityRuleById";
    
    internal static WebApplication MapActivityRulesEndpoints(this WebApplication app)
    {
        app.MapPost($"api/{ActivityRulesTag}", async (ActivityRuleRequestDto command, IHttpContextAccessor httpContext, 
                ICqrsDispatcher dispatcher, CancellationToken cancellationToken, IIdentityContext identityContext) => 
            {
                var activityRuleId = ActivityRuleId.New();
                var userId = identityContext.GetUser();

                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                
                await dispatcher.HandleAsync(command.MapAsCommand(userId.Value, activityRuleId), cancellationToken);
                httpContext.AddResourceIdHeader(activityRuleId.ToString());
                
                return Results.CreatedAtRoute(nameof(GetActivityRuleById), new {activityRuleId = activityRuleId.ToString()}, null);
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("CreateActivityRule")
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Adds activity rule"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);

        app.MapPost($"api/{ActivityRulesTag}/{{activityRuleId:ulid}}/stages", async (Ulid activityRuleId, 
                CreateStageRequestDto dto, 
                ICqrsDispatcher dispatcher, 
                IIdentityContext identityContext, 
                CancellationToken cancellationToken) =>
            {
                var userId = identityContext.GetUser();

                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                
                var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
                var stageId = StageId.New();
                var command = dto.MapAsCommand(userId.Value, stronglyActivityRuleId, stageId);
                await dispatcher.HandleAsync(command, cancellationToken);

                //TODO: Should returns 201
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status201Created, typeof(void))
            .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
            .WithName("CreateStageForActivityRule")
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Creates new stage for activity rule"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);

        app.MapPut($"api/{ActivityRulesTag}/{{activityRuleId:ulid}}", async (Ulid activityRuleId, UpdateActivityRuleDto dto,
            CancellationToken cancellationToken, ICqrsDispatcher dispatcher, IIdentityContext identityContext) =>
        {
            var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
            var userId = identityContext.GetUser();

            if (userId is null)
            {
                return Results.Unauthorized();
            }
            
            await dispatcher.HandleAsync(dto.AsCommand(userId.Value, stronglyActivityRuleId), cancellationToken);

            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent, typeof(void))
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status401Unauthorized, typeof(void))
        .Produces(StatusCodes.Status403Forbidden, typeof(void))
        .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
        .WithName("UpdateActivityRule")
        .WithTags(ActivityRulesTag)
        .WithOpenApi(operation => new (operation)
        {
            Description = "Adds activity rule"
        })
        .RequireAuthorization()
        .RequireAuthorization(UserStatePolicy.Name);

        app.MapDelete($"api/{ActivityRulesTag}/{{activityRuleId:ulid}}", async (Ulid activityRuleId, 
                CancellationToken cancellationToken, ICqrsDispatcher dispatcher, IIdentityContext identityContext) =>
        {
            var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
            var userId = identityContext.GetUser();

            if (userId is null)
            {
                return Results.Unauthorized();
            }
            
            await dispatcher.HandleAsync(new DeleteActivityRuleCommand(userId.Value, stronglyActivityRuleId), cancellationToken);

            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent, typeof(void))
        .Produces(StatusCodes.Status401Unauthorized, typeof(void))
        .Produces(StatusCodes.Status403Forbidden, typeof(void))
        .WithName("DeleteActivityRule")
        .WithTags(ActivityRulesTag)
        .WithOpenApi(operation => new (operation)
        {
            Description = "Removes activity rule"
        })
        .RequireAuthorization()
        .RequireAuthorization(UserStatePolicy.Name);
        
        app.MapDelete($"api/{ActivityRulesTag}/{{activityRuleId:ulid}}/stages/{{stageId:ulid}}", async (Ulid activityRuleId, 
                Ulid stageId, CancellationToken cancellationToken, ICqrsDispatcher dispatcher, IIdentityContext identityContext) =>
            {
                var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
                var stronglyStageId = new StageId(stageId);
                var userId = identityContext.GetUser();

                if (userId is null)
                {
                    return Results.Unauthorized();
                }
            
                await dispatcher.HandleAsync(new DeleteActivityRuleStageCommand(userId.Value, stronglyActivityRuleId, stronglyStageId), cancellationToken);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName("DeleteActivityRuleStag")
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Removes activity rule stage"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);

        app.MapGet($"api/{ActivityRulesTag}", async (CancellationToken cancellationToken, 
                ICqrsDispatcher dispatcher, IIdentityContext identityContext) =>
            {   
                var userId = identityContext.GetUser();

                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                
                var result = await dispatcher.SendAsync(new GetActivityRulesQuery(userId.Value), cancellationToken);
                
                return Results.Ok(result);
            })            
            .Produces(StatusCodes.Status200OK, typeof(IReadOnlyCollection<ActivityRuleResponseDto>))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status403Forbidden, typeof(void))
            .WithName("GetActivityRules")
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
                Description = "Get activity rules collection" 
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        app.MapGet($"api/{ActivityRulesTag}/{{activityRuleId:ulid}}", async (Ulid activityRuleId,
                CancellationToken cancellationToken, ICqrsDispatcher dispatcher, IIdentityContext identityContext) =>
            {   
                var stronglyActivityRuleId = new ActivityRuleId(activityRuleId);
                var userId = identityContext.GetUser();

                if (userId is null)
                {
                    return Results.Unauthorized();
                }
                
                var result = await dispatcher.SendAsync(new GetActivityRuleByIdQuery(userId.Value, stronglyActivityRuleId), cancellationToken);
                
                return result is null ? Results.NotFound() : Results.Ok(result);
            })            
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
            .WithName(GetActivityRuleById)
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new (operation)
            {
               Description = "Get activity rule by id" 
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);

        app.MapGet($"api/{ActivityRulesTag}/modes", async (CancellationToken cancellationToken,
                ICqrsDispatcher dispatcher) =>
            {
                var result = await dispatcher.SendAsync(new GetRuleModesQuery(), cancellationToken);
                return Results.Ok(result);
            })
            .Produces(StatusCodes.Status200OK, typeof(void))
            .Produces(StatusCodes.Status401Unauthorized, typeof(void))
            .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
            .WithName("GetActivityRuleModes")
            .WithTags(ActivityRulesTag)
            .WithOpenApi(operation => new(operation)
            {
                Description = "Retrieves all activity rules modes"
            })
            .RequireAuthorization()
            .RequireAuthorization(UserStatePolicy.Name);
        
        return app;
    }
}