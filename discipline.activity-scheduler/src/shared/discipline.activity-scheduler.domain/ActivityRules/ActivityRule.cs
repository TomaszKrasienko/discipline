using discipline.activity_scheduler.shared.abstractions.Identifiers;

namespace discipline.activity_scheduler.domain.ActivityRules;

public sealed class ActivityRule
{
    public ActivityRuleId ActivityRuleId { get; }
    public AccountId AccountId { get; }
    public string Title { get; private set; }
    public string Mode { get; private set; }
    public IReadOnlyCollection<int>? SelectedDays { get; private set; }

#pragma warning disable CS8618, CS9264
    public ActivityRule() {}
#pragma warning restore CS8618, CS9264
    
    private ActivityRule(
        ActivityRuleId activityRuleId, 
        AccountId accountId,
        string title, 
        string mode,
        IReadOnlyCollection<int>? selectedDays)
    {
        ActivityRuleId = activityRuleId;
        AccountId = accountId;
        Title = title;
        Mode = mode;
        SelectedDays = selectedDays;
    }

    public static ActivityRule Create(
        ActivityRuleId activityRuleId,
        AccountId accountId,
        string title, 
        string mode, 
        IReadOnlyCollection<int>? selectedDays) 
        => new(activityRuleId, 
            accountId,
            title,
            mode,
            selectedDays);

    public void UpdateMode(
        string title,
        string mode,
        IReadOnlyList<int>? days)
    {
        throw new NotImplementedException();
    }
}