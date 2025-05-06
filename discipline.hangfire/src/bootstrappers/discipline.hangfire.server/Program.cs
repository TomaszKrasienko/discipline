using discipline.hangfire.create_activity_from_planned.Configuration;
using discipline.hangfire.infrastructure.Configuration;
using discipline.hangfire.server.Hangfire;
using discipline.hangfire.shared.abstractions.Api;
using Hangfire;

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
    .SetAddActivityRules(builder.Configuration)
    .SetAddPlannedTasks(builder.Configuration)
    .SetCreateActivityFromPlanned();

builder.UseInfrastructure();

var app = builder.Build();
app.UseDisciplineHangfireServer();
app.UseHttpsRedirection();

RecurringJob.AddOrUpdate<IAddPlannedTasksApi>(
    "execute-task-planning",
    job => job.ExecuteTaskPlanning(CancellationToken.None), 
    Cron.Daily);

RecurringJob.AddOrUpdate<IAddActivityRulesApi>(
    "get-incorrect-activity-rules",
    job => job.GetIncorrectActivityRulesAsync(CancellationToken.None),
    Cron.Hourly);

RecurringJob.AddOrUpdate<ICreateActivityFromPlannedApi>(
    "create-activity-from-planned",
    job => job.ExecuteTaskCreating(CancellationToken.None),
    Cron.Daily);

await app.RunAsync();