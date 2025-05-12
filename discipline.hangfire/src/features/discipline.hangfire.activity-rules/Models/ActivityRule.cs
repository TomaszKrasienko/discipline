using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.activity_rules.Models;

internal sealed class ActivityRule
{
    public ActivityRuleId ActivityRuleId { get; }
    public UserId UserId { get; }
    public string? Title { get; private set; }
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

    internal void Set(string title, 
        string mode, 
        IReadOnlyCollection<int>? selectedDays)
    {
        Title = title;
        Mode = mode;
        SelectedDays = selectedDays;
    }

    internal void UpdateMode(string mode, IReadOnlyList<int>? days = null)
    {
        Mode = mode;
        SelectedDays = days;
    }
}