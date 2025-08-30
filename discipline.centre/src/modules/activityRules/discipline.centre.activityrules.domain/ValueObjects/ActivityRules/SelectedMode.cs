using System.Collections.Immutable;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Rules.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.activityrules.domain.ValueObjects.ActivityRules;

public sealed class SelectedMode : ValueObject
{
    public RuleMode Mode { get; }
    public IReadOnlySet<int>? Days { get; }

    public static SelectedMode Create(RuleMode mode, HashSet<int>? days)
    {
        CheckRule(new RuleModeRequireSelectedDaysRule(mode, days));
        foreach (var day in days ?? [])
        {
            CheckRule(new SelectedDayCanNotBeOutOfRangeRule(day));
        }

        var selectedDays = days?.Select(x => x)?.ToImmutableHashSet();
        
        return new SelectedMode(mode, selectedDays);
    }
    
    private SelectedMode(RuleMode mode, IReadOnlySet<int>? days)
    {
        Mode = mode;
        Days = days;
    }
    
    internal bool HasChanges(RuleMode mode, IReadOnlySet<int>? days)
        => Mode != mode
        || days is null ? Days is not null : Days?.SequenceEqual(days) ?? true;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Mode;
        yield return Days;
    }
}