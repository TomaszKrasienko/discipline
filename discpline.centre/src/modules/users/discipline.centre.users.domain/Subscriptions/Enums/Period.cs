namespace discipline.centre.users.domain.Subscriptions.Enums;

public readonly record struct Period
{
    public static IReadOnlyCollection<Period> GetAvailable()
        =>
        [
            Month,
            Year,
        ];
    
    public static Period Month = new("Month");
    public static Period Year = new("Year");
    
    public string Value { get; }

    private Period(string value)
    {
        Value = value;
    }

    public static Period FromValue(string value) => value switch
    {
        nameof(Month) => Month,
        nameof(Year) => Year,
        _ => throw new ArgumentException("Period.InvalidFormat")
    };
};