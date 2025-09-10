using discipline.hangfire.create_empty_daily_tracker.Configuration;
using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.server.Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allAssemblies = AppDomain
    .CurrentDomain
    .GetAssemblies()
    .ToList();

builder.Services
    .AddDisciplineHangfire(builder.Configuration)
    .AddInfrastructure(builder.Configuration, allAssemblies)
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

// RecurringJob.AddOrUpdate<IAddPlannedTasksApi>(
//     "execute-task-planning",
//     job => job.ExecuteTaskPlanning(CancellationToken.None), 
//     Cron.Hourly);
//
// RecurringJob.AddOrUpdate<ICreateEmptyDailyTrackerApi>(
//     "execute-create-empty-daily-tracker",
//     job => job.Generate(CancellationToken.None), 
//     Cron.Daily(3, 00));
//
// RecurringJob.AddOrUpdate<ICreateActivityFromPlannedApi>(
//     "create-activity-from-planned",
//     job => job.ExecuteTaskCreating(CancellationToken.None),
//     Cron.Daily);

await app.RunAsync();