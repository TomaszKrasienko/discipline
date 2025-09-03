using discipline.daily_trackers.domain.DailyTrackers.BusinessRules.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel;

namespace discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;

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

    private Day(DateOnly value) => Value = value;

    public static Day Create(DateOnly value) => new(value);

    //TODO: Unittests
    public Day GetPriorDay()
    {
        var value = _value.AddDays(-1);
        return Create(value);
    }

    public static implicit operator DateOnly(Day day)
        => day.Value;
    
    public static implicit operator Day(DateOnly value)
        => Create(value);
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}