using discipline.activity_scheduler.shared.abstractions.Identifiers;

namespace discipline.activity_scheduler.domain.PlannedTasks;

public sealed class PlannedTask
{
    public PlannedTaskId Id { get; }
    public ActivityRuleId ActivityRuleId { get; }
    public AccountId AccountId { get; }
    public DateOnly PlannedFor { get; }
    public DateTimeOffset CreatedAt { get; }
    public bool PlannedEnabled { get; }
    public bool ActivityCreated { get; }

#pragma warning disable CS8618, CS9264
    /// <summary>
    /// Only for EF purposes
    /// </summary>
    public PlannedTask()
    {
        
    }
#pragma warning restore CS8618, CS9264
    
    private PlannedTask(ActivityRuleId activityRuleId, 
        AccountId accountId, 
        DateOnly plannedFor, 
        DateTimeOffset createdAt, 
        bool plannedEnable,
        bool activityCreated)
    {
        Id = PlannedTaskId.New(); 
        ActivityRuleId = activityRuleId;
        AccountId = accountId;
        PlannedFor = plannedFor;
        CreatedAt = createdAt;
        PlannedEnabled = plannedEnable;
        ActivityCreated = activityCreated;
    }

    public static PlannedTask Create(ActivityRuleId activityRuleId, 
        AccountId accountId, 
        DateOnly plannedFor,
        DateTimeOffset createdAt)
    {
        return new PlannedTask(
            activityRuleId,
            accountId,
            plannedFor,
            createdAt,
            true,
            false);
    }
}