using discipline.hangfire.add_planned_tasks.Facades;
using discipline.hangfire.domain.PlannedTasks;
using discipline.hangfire.shared.abstractions.DAL;
using discipline.hangfire.shared.abstractions.Identifiers;
using discipline.hangfire.shared.abstractions.ViewModels;

namespace discipline.hangfire.add_planned_tasks.Api;

public interface IAddPlannedTasksApi
{
    Task ExecuteTaskPlanning(CancellationToken cancellationToken);
}

internal sealed class AddPlannedTasksApi(ICentreFacade centreFacade,
    TimeProvider timeProvider,
    IWriteRepository<PlannedTask, PlannedTaskId> plannedTaskRepository,
    IReadRepository<ActivityRuleViewModel, ActivityRuleId> activityRuleRepository) : IAddPlannedTasksApi
{
    public async Task ExecuteTaskPlanning(CancellationToken cancellationToken)
    {
        var activeModesResult = await centreFacade.GetActiveModes(cancellationToken);

        if (activeModesResult is { IsT0: true, AsT0: false })
        {
            return;
        }

        var modes = activeModesResult.AsT1.Modes;
        var day = activeModesResult.AsT1.Day;
        
        var tasksToPlan = await activityRuleRepository
            .SearchAsync(x =>
                    modes.Contains(x.Mode!) ||
                    x.SelectedDays!.Contains(day),
                cancellationToken);
        
        var now = timeProvider.GetUtcNow();
        var plannedFor = DateOnly.FromDateTime(now.AddDays(1).Date);
        List<PlannedTask> plannedTasks = [];
        
        foreach (var taskToPlan in tasksToPlan)
        {
            if (await IsPlannedTaskExistsAsync(
                    taskToPlan.ActivityRuleId,
                    taskToPlan.AccountId,
                    plannedFor,
                    cancellationToken))
            {
                continue;
            }
            
            plannedTasks.Add(
                PlannedTask.Create(
                    taskToPlan.ActivityRuleId,
                    taskToPlan.AccountId,
                    plannedFor,
                    now));
        }
        
        await plannedTaskRepository.AddRangeAsync(plannedTasks, cancellationToken);
    }
    
    private Task<bool> IsPlannedTaskExistsAsync(
        ActivityRuleId activityRuleId,
        AccountId accountId,
        DateOnly plannedFor,
        CancellationToken cancellationToken)
        => plannedTaskRepository
            .DoesExistAsync(x => 
                    x.ActivityRuleId == activityRuleId &&
                    x.AccountId == accountId &&
                    x.PlannedFor == plannedFor,
                cancellationToken);
}