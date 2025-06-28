using discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Stages;
using discipline.daily_trackers.domain.SharedKernel;

namespace discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Stages;

public sealed class Title : ValueObject
{
    private readonly string _value = null!;

    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new TitleCanNotBeEmptyRule(value));
            CheckRule(new TitleCannotBeLongerThan30(value));
            _value = value;
        }
    }

    private Title(string value) => Value = value;

    public static Title Create(string value) => new Title(value);

    public static implicit operator string(Title title) => title.Value;

    public static implicit operator Title(string value) => Create(value);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}