using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;

namespace discipline.centre.activityrules.domain.Enums;

public sealed record RuleMode
{
    public static RuleMode EveryDayMode => new("EveryDay", false);
    
    public static RuleMode FirstDayOfWeekMode => new("FirstDayOfWeek", false);
    
    public static RuleMode LastDayOfWeekMode => new("LastDayOfWeek", false);

    public static RuleMode CustomMode => new("Custom", true);

    public static RuleMode FirstDayOfMonth => new("FirstDayOfMonth", false);
    
    public static RuleMode LastDayOfMonthMode => new("LastDayOfMonth", false);
    
    public string Value { get; }
    public bool IsDaysRequired { get; }

    private RuleMode(string value, bool isDaysRequired)
    {
        Value = value;
        IsDaysRequired = isDaysRequired;
    }
    
    
}