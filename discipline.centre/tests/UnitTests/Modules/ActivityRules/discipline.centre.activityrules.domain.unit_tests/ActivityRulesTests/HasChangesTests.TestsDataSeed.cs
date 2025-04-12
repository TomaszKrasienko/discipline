using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class HasChangesTests
{
    public static IEnumerable<object[]> GetHasChangesUnchangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",
                    null), SelectedMode.EveryDayMode, null, []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",
                    "test_note"),SelectedMode.CustomMode, [1,2,3], []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", "test_note"),
                SelectedMode.CustomMode, [1,2,3])
        ];
    }

    public static IEnumerable<object[]> GetHasChangesChangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",
                    null), SelectedMode.EveryDayMode, null, []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title1", null),
                SelectedMode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    null), SelectedMode.EveryDayMode, null, []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", "test_note"),
                SelectedMode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",
                    "test_note"), SelectedMode.EveryDayMode, null, []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", "test_note1"),
                SelectedMode.EveryDayMode, null)
        ];
                
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    "test_note"), SelectedMode.EveryDayMode, null, []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", "test_note"),
                SelectedMode.FirstDayOfMonth, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",
                    null), SelectedMode.CustomMode, [1,2], []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    null),SelectedMode.CustomMode, [1,2], []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.CustomMode, [2,3])
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    null),SelectedMode.EveryDayMode, null, []),
            new HasChangesParameters(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.CustomMode, [1,2])
        ];
    }

    public sealed record HasChangesParameters(ActivityRuleDetailsSpecification Details, string Mode, List<int>? SelectedDays);
}