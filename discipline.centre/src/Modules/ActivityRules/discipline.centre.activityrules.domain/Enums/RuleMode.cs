using System.Diagnostics;
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

    //TODO: Tests
    public static RuleMode Parse(string value) => value switch
    {
        nameof(EveryDayMode) => EveryDayMode,
        nameof(FirstDayOfWeekMode) => FirstDayOfWeekMode,
        nameof(LastDayOfWeekMode) => LastDayOfWeekMode,
        nameof(CustomMode) => CustomMode,
        nameof(FirstDayOfMonth) => FirstDayOfMonth,
        nameof(LastDayOfMonthMode) => LastDayOfMonthMode,
        //TODO: Custom exception
        _ => throw new ArgumentException($"Invalid value: {value}")
    };
}