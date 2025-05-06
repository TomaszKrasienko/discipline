using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.add_planned_tasks.Models;

internal sealed class PlannedTask
{
    public Ulid Id { get; }
    public ActivityRuleId ActivityRuleId { get; }
    public UserId UserId { get; }
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
        UserId userId, 
        DateOnly plannedFor, 
        DateTimeOffset createdAt, 
        bool isPlannedEnable,
        bool isActivityCreated)
    {
        Id = Ulid.NewUlid(); 
        ActivityRuleId = activityRuleId;
        UserId = userId;
        PlannedFor = plannedFor;
        CreatedAt = createdAt;
        IsPlannedEnable = isPlannedEnable;
        IsActivityCreated = isActivityCreated;
    }

    internal static PlannedTask Create(ActivityRuleId activityRuleId, 
        UserId userId, 
        DateOnly plannedFor,
        DateTimeOffset createdAt)
    {
        return new PlannedTask(activityRuleId, userId, plannedFor, createdAt, true, false);
    }
    
    internal void MarkAsActivityCreated()
        => IsActivityCreated = true;
}