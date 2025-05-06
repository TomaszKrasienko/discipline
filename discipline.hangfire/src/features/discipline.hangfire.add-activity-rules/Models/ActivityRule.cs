using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.add_activity_rules.Models;

internal sealed class ActivityRule
{
    public ActivityRuleId ActivityRuleId { get; }
    public UserId UserId { get; }
    public string? Mode { get; private set; }
    public IReadOnlyCollection<int>? SelectedDays { get; private set; }

#pragma warning disable CS8618, CS9264
    public ActivityRule() {}
#pragma warning restore CS8618, CS9264
    
    private ActivityRule(ActivityRuleId activityRuleId, 
        UserId userId)
    {
        ActivityRuleId = activityRuleId;
        UserId = userId;
    }

    internal static ActivityRule Create(ActivityRuleId activityRuleId,
        UserId userId) 
        => new(activityRuleId, userId);

    internal void Set(string mode, IReadOnlyCollection<int>? selectedDays)
    {
        Mode = mode;
        SelectedDays = selectedDays;
    }
}