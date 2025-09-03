using discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Stages;
using discipline.daily_trackers.domain.SharedKernel;

namespace discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Stages;

public sealed class OrderIndex : ValueObject
{
    private readonly int _index;

    public int Value
    {
        get => _index;
        private init
        {
            CheckRule(new IndexCannotBeLessThan1Rule(value));
            _index = value;
        }
    }

    private OrderIndex(int value)
        => Value = value;

    public static OrderIndex Create(int value)
        => new (value);
    
    public static implicit operator int (OrderIndex index)
        => index.Value;
    
    public static implicit operator OrderIndex(int value)
        => Create(value);
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}