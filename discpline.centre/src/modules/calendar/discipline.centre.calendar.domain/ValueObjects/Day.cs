using discipline.centre.calendar.domain.Rules.BaseCalendarEvents;
using discipline.centre.calendar.domain.Rules.UserCalendarDays;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.calendar.domain.ValueObjects;

public sealed class Day : ValueObject
{
    private readonly DateOnly _value;
    public DateOnly Value
    {
        get => _value;
        private init
        {
            CheckRule(new DayCannotBeDefaultRule(value));
            _value = value;
        }
    }

    private Day(DateOnly value)
        => Value = value;

    internal static Day Create(DateOnly value)
        => new (value);

    public static implicit operator DateOnly(Day day)
        => day.Value;
    
    public static implicit operator Day(DateOnly value)
        => Create(value);
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}