using System.Reflection;
using discipline.daily_trackers.application.UserDailyTrackers.Commands;
using discipline.daily_trackers.application.UserDailyTrackers.DTOs.Requests;
using discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;
using discipline.daily_trackers.application.UserDailyTrackers.Queries;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.daily_trackers.infrastructure.Configuration;
using discipline.daily_trackers.infrastructure.IdentityContext.Abstractions;
using discipline.libs.cqrs.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

const string tag = "daily-trackers";
const string getById = "getUserDailyTrackerByIdQuery"; 

var builder = WebApplication.CreateBuilder(args);

var basePath = AppContext.BaseDirectory;
var assemblies = Directory.GetFiles(basePath, "discipline.daily-trackers*.dll")
    .Select(Assembly.LoadFrom)
    .ToList();

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddInfrastructure(builder.Configuration, assemblies)
    .AddDomain();

var app = builder.Build();

app.UseInfrastructure();

app.MapPost(
    "/api/user-daily-trackers/activities",
    async (CreateActivityRequestDto request,
        IValidator<CreateActivityRequestDto> validator,
        ICqrsDispatcher dispatcher,
        IIdentityContext identityContext,
        IHttpContextAccessor httpContext,
        CancellationToken cancellationToken) =>
    {
        var account = identityContext.GetAccount();

        if (account is null)
        {
            return Results.Unauthorized();
        }
        
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.UnprocessableEntity(validationResult.ToProblemDetails());
        }

        var activityId = ActivityId.New();
        await dispatcher.HandleAsync(request.ToCommand(
            account.Value,
            activityId), cancellationToken);
        httpContext.AddResourceIdHeader(activityId.ToString());
        
        return Results.Ok(activityId);
    })
    .Produces(StatusCodes.Status201Created, typeof(void))
    .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
    .Produces(StatusCodes.Status401Unauthorized, typeof(void))
    .Produces(StatusCodes.Status403Forbidden, typeof(void))
    .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
    .WithName("CreateActivity")
    .WithTags(tag)
    .WithOpenApi(operation => new (operation)
    {
        Description = "Adds activity"
    })
    .RequireAuthorization();

app.MapPost("/api/user-daily-trackers/{day:dateonly}",
    async (DateOnly day,
        ICqrsDispatcher cqrsDispatcher,
        IIdentityContext identityContext,
        IHttpContextAccessor httpContext,
        CancellationToken cancellationToken) =>
    {
        var account = identityContext.GetAccount();

        if (account is null)
        {
            return Results.Unauthorized();
        }

        var dailyTrackerId = DailyTrackerId.New();

        await cqrsDispatcher.HandleAsync(new CreateUserDailyTrackerCommand(
            dailyTrackerId,
            account.Value,
            day), cancellationToken);
        
        httpContext.AddResourceIdHeader(dailyTrackerId.ToString());
        return Results.CreatedAtRoute(getById, new { day = dailyTrackerId.ToString() });
    })    
    .Produces(StatusCodes.Status201Created, typeof(void))
    .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
    .Produces(StatusCodes.Status401Unauthorized, typeof(void))
    .Produces(StatusCodes.Status403Forbidden, typeof(void))
    .Produces(StatusCodes.Status422UnprocessableEntity, typeof(ProblemDetails))
    .WithName("CreateUserDailyTracker")
    .WithTags(tag)
    .WithOpenApi(operation => new (operation)
    {
        Description = "Creates empty user daily tracker"
    })
    .RequireAuthorization();

app.MapGet(
    "/api/user-daily-trackers/{day:dateonly}",
    async (DateOnly day,
        ICqrsDispatcher cqrsDispatcher,
        IIdentityContext identityContext,
        CancellationToken cancellationToken) =>
    {
        var account = identityContext.GetAccount();

        if (account is null)
        {
            return Results.Unauthorized();
        }

        var query = new GetUserDailyTrackerByDayQuery(
            account.Value,
            day);
        
        var result = await cqrsDispatcher.SendAsync(query, cancellationToken);
        
        return result is null ? Results.NotFound() : Results.Ok(result);
    })
    .Produces(StatusCodes.Status200OK, typeof(UserDailyTrackerResponseDto))
    .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
    .Produces(StatusCodes.Status401Unauthorized, typeof(void))
    .Produces(StatusCodes.Status403Forbidden, typeof(void))
    .WithName("GetUserDailyTrackerByDay")
    .WithTags(tag)
    .WithOpenApi(operation => new (operation)
    {
        Description = "Retrieves user daily tracker by day"
    })
    .RequireAuthorization();

app.MapGet(
    "/api/user-daily-trackers/{id:ulid}",
    async (Ulid id,
        ICqrsDispatcher cqrsDispatcher,
        IIdentityContext identityContext,
        CancellationToken cancellationToken) =>
    {
        var account = identityContext.GetAccount();

        if (account is null)
        {
            return Results.Unauthorized();
        }

        var query = new GetUserDailyTrackerByIdQuery(
            account.Value,
            new DailyTrackerId(id));
    
        var result = await cqrsDispatcher.SendAsync(query, cancellationToken);
    
        return result is null ? Results.NotFound() : Results.Ok(result);
    })
    .Produces(StatusCodes.Status200OK, typeof(UserDailyTrackerResponseDto))
    .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
    .Produces(StatusCodes.Status401Unauthorized, typeof(void))
    .Produces(StatusCodes.Status403Forbidden, typeof(void))
    .WithName(getById)
    .WithTags(tag)
    .WithOpenApi(operation => new (operation)
    {
        Description = "Retrieves user daily tracker by id"
    })
    .RequireAuthorization();

app.UseHttpsRedirection();

app.Run();