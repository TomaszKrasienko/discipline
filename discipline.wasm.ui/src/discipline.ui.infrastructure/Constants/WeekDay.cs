namespace discipline.ui.infrastructure.Constants;

public record WeekDay
{
    public static WeekDay Sunday { get; } = new WeekDay(0, "Sunday");
    public static WeekDay Monday { get; } = new WeekDay(1, "Monday");
    public static WeekDay Tuesday { get; } = new WeekDay(2, "Tuesday");
    public static WeekDay Wednesday { get; } = new WeekDay(3, "Wednesday");
    public static WeekDay Thursday { get; } = new WeekDay(4, "Thursday");
    public static WeekDay Friday { get; } = new WeekDay(5, "Friday");
    public static WeekDay Saturday { get; } = new WeekDay(6, "Saturday");
    
    public string Name { get; }
    public int Value { get; }

    private WeekDay(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public static IEnumerable<WeekDay> List()
        => [Sunday, Monday,  Tuesday, Wednesday, Thursday,  Friday, Saturday];
    
    public static WeekDay FromName(string name)
        => List().SingleOrDefault(x => x.Name == name) ?? throw new ArgumentException("Invalid week day name");
    
    public static WeekDay FromValue(int value)
        => List().SingleOrDefault(x => x.Value == value) ?? throw new ArgumentException("Invalid week day value");
}