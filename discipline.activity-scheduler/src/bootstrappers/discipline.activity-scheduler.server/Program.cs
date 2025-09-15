using System.Reflection;
using discipline.activity_scheduler.add_planned_tasks.Api;
using discipline.activity_scheduler.create_activity_from_planned.Apis;
using discipline.activity_scheduler.create_empty_daily_tracker.Api;
using discipline.activity_scheduler.create_empty_daily_tracker.Configuration;
using discipline.activity_scheduler.server.Hangfire;
using discipline.activity_scheduler.shared.infrastructure.Configuration;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var basePath = AppContext.BaseDirectory;
var assemblies = Directory.GetFiles(basePath, "discipline.hangfire*.dll")
    .Select(Assembly.LoadFrom)
    .ToList();

builder.Services
    .AddDisciplineHangfire(builder.Configuration)
    .AddInfrastructure(builder.Configuration, assemblies)
    .SetAccountModification()
    .SetAddActivityRules(builder.Configuration)
    .SetAddPlannedTasks(builder.Configuration)
    .SetCreateEmptyUserDailyTracker();

builder.UseInfrastructure();

var app = builder.Build();

// app.MapGet("/api/planned-tasks/{userId}", async (string userId, 
//     IBrowsePlannedApi browsePlannedApi,
//     CancellationToken cancellationToken) =>
//     {
//         var stronglyUserId = AccountId.Parse(userId);
//         var result = await browsePlannedApi
//             .GetPlannedTaskDetailsAsync(stronglyUserId, cancellationToken);
//         
//         return Results.Ok(result);
//     })
//     .Produces(StatusCodes.Status200OK);

app.UseDisciplineHangfireServer();
app.UseHttpsRedirection();

CancellationToken ct = CancellationToken.None;

RecurringJob.AddOrUpdate<IAddPlannedTasksApi>(
    "execute-task-planning",
    job => job.ExecuteTaskPlanning(ct), 
    Cron.Hourly);

RecurringJob.AddOrUpdate<ICreateEmptyDailyTrackerApi>(
    "execute-create-empty-daily-tracker",
    job => job.Generate(ct), 
    Cron.Daily(3, 00));

RecurringJob.AddOrUpdate<ICreateActivitiesFromPlanned>(
    "execute-create-activities-from-planned",
    job => job.ExecuteAsync(ct),
    Cron.Daily(3, 30));

await app.RunAsync();