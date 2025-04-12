using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public partial class CreateTests
{
    public static IEnumerable<object[]> GetValidCreateActivityRulesData()
    {
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title",null),SelectedMode.EveryDayMode,
                null, [])
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title","test_note"),
                SelectedMode.CustomMode, [1,2,3], [])
        ];

        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", "test_note"),
                SelectedMode.EveryDayMode, null, [new StageSpecification("test_stage1", 1),
                    new StageSpecification("test_stage2", 2)])
        ];
    }
    

    public static IEnumerable<object[]> GetInvalidCreateActivityRulesData()
    {
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification(string.Empty, null),SelectedMode.EveryDayMode, null, []),
                "ActivityRule.Details.Title.Empty"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", null), string.Empty,null, []),
            "ActivityRule.Mode.Empty"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", null), "test_mode",null, []),
            "ActivityRule.Mode.Unavailable"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", null), SelectedMode.CustomMode, 
                [-1, 2, 3], []),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", null), SelectedMode.CustomMode, 
                [1, 7, 3], []),
            "ActivityRule.SelectedDay.OutOfRange"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.EveryDayMode, null, [new StageSpecification("test_stage1", 1),
                    new StageSpecification("test_stage2", 3)]),
            "ActivityRule.Stages.MustHaveOrderedIndex"
        ];
        
        yield return
        [
            new CreateActivityRuleParams(ActivityRuleId.New(), UserId.New(), 
                new ActivityRuleDetailsSpecification("test_title", null),
                SelectedMode.EveryDayMode, null, [new StageSpecification("test_stage1", 1),
                    new StageSpecification("test_stage1", 2)]),
            "ActivityRule.Stages.StageTitleMustBeUnique"
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
    
    public sealed record CreateActivityRuleParams(ActivityRuleId Id, UserId UserId, 
        ActivityRuleDetailsSpecification Details, string Mode,
        List<int>? SelectedDays, List<StageSpecification> Stages);
}