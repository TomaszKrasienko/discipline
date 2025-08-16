using discipline.daily_trackers.domain.SharedKernel;

namespace discipline.daily_trackers.domain.DailyTrackers.ValueObjects;

public sealed class IsChecked : ValueObject
{
    public bool Value { get; }

    private IsChecked(bool value) 
        => Value = value;
    
    public static IsChecked Create(bool value) 
        => new IsChecked(value);

    public static implicit operator bool(IsChecked isChecked)
        => isChecked.Value;

    public static implicit operator IsChecked(bool value)
        => new IsChecked(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}