// using discipline.daily_trackers.domain.DailyTrackers;
// using discipline.daily_trackers.domain.DailyTrackers.Specifications;
// using discipline.daily_trackers.domain.SharedKernel.Exceptions;
// using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
// using Shouldly;
//
// namespace discipline.daily_trackers.domain.tests.ActivityTests;
//
// public sealed class AddStageTests
// {
//     [Fact]
//     public void GivenUniqueTitle_WhenAddStage_ThenAddNewStage()
//     {
//         // Arrange
//         var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
//             null), null, null);
//         
//         // Act
//         _ = activity.AddStage("Test_title");
//         
//         // Assert
//         activity.Stages.Any().ShouldBeTrue();
//     } 
//     
//     [Fact]
//     public void GivenEmptyStages_WhenAddStage_ThenReturnStageWithFirstIndex()
//     {
//         // Arrange
//         var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
//             null), null, null);
//         
//         // Act
//         var stage = activity.AddStage("Test_title");
//         
//         // Assert
//         stage.Index.Value.ShouldBe(1);
//     } 
//     
//     [Fact]
//     public void GivenExistingStages_WhenAddStage_ThenReturnStageWithNextIndex()
//     {
//         // Arrange
//         var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
//             null), null, null);
//         
//         _ = activity.AddStage("Test_title");
//         
//         // Act
//         var stage = activity.AddStage("Test_title_second");
//         
//         // Assert
//         stage.Index.Value.ShouldBe(2);
//     } 
//     
//     [Fact]
//     public void GivenAlreadyRegisteredStageTitle_WhenAddStage_ThenThrowDomainExceptionWithCodeActivityRuleStagesStageTitleMustBeUnique()
//     {
//         // Arrange
//         var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
//             null), null, null);
//         const string title = "test_stage_title";
//         
//         activity.AddStage(title);
//         
//         // Act
//        var exception = Record.Exception(() => activity.AddStage(title));
//         
//         // Assert
//         exception.ShouldBeOfType<DomainException>();
//         ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.StageTitleMustBeUnique");
//     } 
// }