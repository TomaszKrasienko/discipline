using discipline.hangfire.add_planned_tasks.DAL;
using discipline.hangfire.add_planned_tasks.Facades;
using discipline.hangfire.add_planned_tasks.Models;
using discipline.hangfire.shared.abstractions.Api;
using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace discipline.hangfire.add_planned_tasks;

internal sealed class AddPlannedTasksApi(ICentreFacade centreFacade,
    IActivityRulesApi addActivityRulesApi,
    TimeProvider timeProvider,
    AddPlannedTaskDbContext dbContext) : IAddPlannedTasksApi
{
    public async Task ExecuteTaskPlanning(CancellationToken cancellationToken)
    {
        var activeModesResult = await centreFacade.GetActiveModes(cancellationToken);

        if (activeModesResult is { IsT0: true, AsT0: false })
        {
            return;
        }
        
        var tasksToPlan = await addActivityRulesApi
            .GetActivityRulesByModesAsync(activeModesResult.AsT1.Modes, activeModesResult.AsT1.Day, cancellationToken);

        var now = timeProvider.GetUtcNow();
        var plannedFor = DateOnly.FromDateTime(now.AddDays(1).Date);
        
        foreach (var taskToPlan in tasksToPlan)
        {
            if (await IsPlannedTaskExistsAsync(taskToPlan.ActivityRuleId, taskToPlan.UserId, plannedFor,
                    cancellationToken))
            {
                continue;
            }
            
            var task = PlannedTask.Create(taskToPlan.ActivityRuleId, taskToPlan.UserId, plannedFor, now);
                
            dbContext.Add(task);
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<PlannedTaskViewModel>> GetPlannedTask(DateOnly date, CancellationToken cancellationToken)
    {
        var plannedTasks = await dbContext.Set<PlannedTask>()
                .AsNoTracking()
                .Where(x 
                    => x.PlannedFor == date
                    && x.IsPlannedEnable)
                .ToListAsync(cancellationToken);

        return plannedTasks.Select(x => new PlannedTaskViewModel(x.Id, x.ActivityRuleId, x.UserId)).ToArray();
    }

    public async Task MarkAsPlanned(Ulid plannedTaskId, CancellationToken cancellationToken)
    {
        var plannedTask = await dbContext.Set<PlannedTask>()
            .SingleOrDefaultAsync(x => x.Id == plannedTaskId, cancellationToken);
        plannedTask?.MarkAsActivityCreated();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private Task<bool> IsPlannedTaskExistsAsync(ActivityRuleId activityRuleId, UserId userId,
        DateOnly plannedFor, CancellationToken cancellationToken)
        => dbContext.Set<PlannedTask>()
            .AnyAsync(x 
                => x.ActivityRuleId == activityRuleId 
                && x.UserId == userId 
                && x.PlannedFor == plannedFor, cancellationToken);
}