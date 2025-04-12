using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class EditTests
{
    public static IEnumerable<object[]> GetValidEditActivityRulesData()
    {
        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("new_test_title", null), SelectedMode.EveryDayMode, null)
        ];

        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("new_test_title", "new_test_title")
                ,SelectedMode.CustomMode, [1,2,3])
        ];
    }
    
    public static IEnumerable<object[]> GetInvalidEditActivityRulesData()
    {
        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification(string.Empty, null),
                SelectedMode.CustomMode, [1, 2, 3]),
            "ActivityRule.Details.Title.Empty"
        ];
        
        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification(new string('t', 31), null),
                SelectedMode.CustomMode, [1, 2, 3]),
            "ActivityRule.Details.Title.TooLong"
        ];
        
        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null),
                string.Empty, [1, 2, 3]),
            "ActivityRule.Mode.Empty"
        ];
        
        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null)
                , "test_mode", [1, 2, 3]),
            "ActivityRule.Mode.Unavailable"
        ];
        
        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.CustomMode, [-1, 2, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.CustomMode, [1, 7, 3]),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
    }
    
    public static IEnumerable<object[]> GetValidModesForSelectedDays()
    {
        yield return [SelectedMode.CustomMode];
    }
    
    public static IEnumerable<object[]> GetInvalidModesForSelectedDays()
    {
        yield return [SelectedMode.EveryDayMode];
        yield return [SelectedMode.FirstDayOfWeekMode];
        yield return [SelectedMode.LastDayOfWeekMode];
        yield return [SelectedMode.FirstDayOfMonth];
        yield return [SelectedMode.LastDayOfMonthMode];
    }
    
    public static IEnumerable<object[]> GetEditUnchangedParameters()
    {
        yield return
        [ 
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",null)
                , SelectedMode.EveryDayMode, null, []),
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    "test_note"), SelectedMode.CustomMode, [1,2,3], []),
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", "test_note"),
                SelectedMode.CustomMode, [1,2,3])
        ];
    }

    public static IEnumerable<object[]> GetEditChangedParameters()
    {
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    null),SelectedMode.EveryDayMode, null, []),
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title1", null),
                SelectedMode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title",
                    "test_note"),SelectedMode.EveryDayMode, null, []),
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null)
                ,SelectedMode.EveryDayMode, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    "test_note"),SelectedMode.EveryDayMode, null, []),
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", "test_note1"),
                SelectedMode.EveryDayMode, null)
        ];      
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    null),SelectedMode.EveryDayMode, null, []),
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.FirstDayOfMonth, null)
        ];
        
        yield return
        [
            ActivityRule.Create(ActivityRuleId.New(), UserId.New(), new ActivityRuleDetailsSpecification("test_title", 
                    null),SelectedMode.CustomMode, [1,2], []),
            new EditActivityRuleParams(new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.CustomMode, [1])
        ];
    }
    
    public sealed record EditActivityRuleParams(ActivityRuleDetailsSpecification Details, string Mode,
        List<int>? SelectedDays);
}