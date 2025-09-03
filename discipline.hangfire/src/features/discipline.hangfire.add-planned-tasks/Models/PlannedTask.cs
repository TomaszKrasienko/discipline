using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.add_planned_tasks.Models;

internal sealed class PlannedTask
{
    public Ulid Id { get; }
    public ActivityRuleId ActivityRuleId { get; }
    public AccountId AccountId { get; }
    public DateOnly PlannedFor { get; }
    public DateTimeOffset CreatedAt { get; }
    public bool IsPlannedEnable { get; }
    public bool IsActivityCreated { get; private set; }

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
        bool isPlannedEnable,
        bool isActivityCreated)
    {
        Id = Ulid.NewUlid(); 
        ActivityRuleId = activityRuleId;
        AccountId = accountId;
        PlannedFor = plannedFor;
        CreatedAt = createdAt;
        IsPlannedEnable = isPlannedEnable;
        IsActivityCreated = isActivityCreated;
    }

    internal static PlannedTask Create(ActivityRuleId activityRuleId, 
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
    
    internal void MarkAsActivityCreated()
        => IsActivityCreated = true;
}