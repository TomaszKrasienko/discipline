using System.Collections.Immutable;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Rules;
using discipline.centre.activityrules.domain.Rules.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.domain.ValueObjects.ActivityRules;

public sealed class SelectedMode : ValueObject
{
    public RuleMode Mode { get; }
    public List<DayOfWeek>? Days { get; }

    public SelectedMode Create(RuleMode mode, List<DayOfWeek>? days)
    {
        return new SelectedMode(mode, days);
    }
    
    private SelectedMode(RuleMode mode, List<DayOfWeek>? days)
    {
        Mode = mode;
        Days = days;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Mode;
        yield return Days;
    }
}