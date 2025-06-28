namespace discipline.centre.activityrules.domain.Enums;

public sealed record RuleMode
{
    public static IEnumerable<RuleMode> AvailableModes =
    [
        EveryDay, FirstDayOfWeek, LastDayOfWeek, Custom, FirstDayOfMonth, LastDayOfMonth
    ];
    
    public static RuleMode EveryDay => new("EveryDay", false);
    
    public static RuleMode FirstDayOfWeek => new("FirstDayOfWeek", false);
    
    public static RuleMode LastDayOfWeek => new("LastDayOfWeek", false);

    public static RuleMode Custom => new("Custom", true);

    public static RuleMode FirstDayOfMonth => new("FirstDayOfMonth", false);
    
    public static RuleMode LastDayOfMonth => new("LastDayOfMonth", false);
    
    public string Value { get; }
    public bool IsDaysRequired { get; }

    private RuleMode(string value, bool isDaysRequired)
    {
        Value = value;
        IsDaysRequired = isDaysRequired;
    }

    //TODO: Tests
    public static RuleMode Parse(string value)
        => AvailableModes.FirstOrDefault(x => x.Value == value)
           ?? throw new ArgumentException($"Invalid value: {value}");

}