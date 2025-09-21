// using discipline.daily_trackers.domain.DailyTrackers;
// using discipline.daily_trackers.domain.DailyTrackers.Specifications;
// using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
// using Shouldly;
//
// namespace discipline.daily_trackers.domain.tests.ActivityTests;
//
// public sealed class ClearParentRuleIdTests
// {
//     [Fact]
//     public void GivenActivity_WhenClearParentActivityRuleIdIsCalled_ThenShouldChangeParentRuleIdToNull()
//     {
//         // Arrange
//         var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test", null),
//             ActivityRuleId.New(), []);
//         
//         // Act
//         activity.ClearParentActivityRuleId();
//         
//         // Assert
//         activity.ParentActivityRuleId.ShouldBeNull();
//     }
// }