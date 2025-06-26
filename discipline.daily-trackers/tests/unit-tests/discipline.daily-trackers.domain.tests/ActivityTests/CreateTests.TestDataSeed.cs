using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.tests.ActivityTests;

public partial class CreateTests
{
    public static IEnumerable<object[]> GetValidCreateData()
    {
        yield return
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                "test_activity_title", null), null, null)
        ];

        yield return
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                "test_activity_title", "test_activity_note"), ActivityRuleId.New(), null)
        ];
    }

    public static IEnumerable<object[]> GetInvalidCreateData()
    {
        yield return 
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                string.Empty, null), null, null),
            "DailyTracker.Activity.Details.Title.Empty"
        ];
    
        yield return 
        [
            new CreateTestParameters(ActivityId.New(), new ActivityDetailsSpecification(
                new string('t', 31), null), null, null),
            "DailyTracker.Activity.Details.Title.TooLong"
        ];
    }

    public sealed record CreateTestParameters(
        ActivityId ActivityId,
        ActivityDetailsSpecification Details,
        ActivityRuleId? ActivityRuleId,
        List<StageSpecification>? Stages);
}
