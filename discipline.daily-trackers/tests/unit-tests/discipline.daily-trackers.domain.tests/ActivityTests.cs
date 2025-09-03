using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.daily_trackers.tests.shared_kernel.Domain;
using Shouldly;

namespace discipline.daily_trackers.domain.tests;

public sealed class ActivityTests
{
    #region Create

    [Fact]
    public void GivenValidArgumentsForActivity_WhenCreate_ThenReturnsActivity()
    {
        // Arrange
        var activityId = ActivityId.New();
        var activityDetails = new ActivityDetailsSpecification(
            "test_activity_title",
            null);
        
        // Act
        var result = Activity.Create(
            activityId,
            activityDetails);
        
        // Assert
        result.Id.ShouldBe(activityId);
        result.Details.Title.ShouldBe(activityDetails.Title);
        result.Details.Note.ShouldBe(activityDetails.Note);
        result.ParentActivityRuleId.ShouldBeNull();
        result.IsChecked.Value.ShouldBeFalse();
        result.Stages.Count.ShouldBe(0);
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerActivityDetailsTitleEmpty()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(
            ActivityId.New(), 
            new ActivityDetailsSpecification(string.Empty, null)));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Details.Title.Empty");
    }
    
    [Fact]
    public void GivenTitleLongerThan30Characters_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerActivityDetailsTitleTooLong()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(
            ActivityId.New(), 
            new ActivityDetailsSpecification(new string( 't', 31), null)));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Details.Title.TooLong");
    }
    
    #endregion
    #region CreateFromRule

    [Fact]
    public void GivenValidArgumentsForActivityFromRule_WhenCreateFromRule_ShouldReturnActivityActivityRuleId()
    {
        // Arrange
        var activityRuleId = ActivityRuleId.New();
        
        // Act
        var result = Activity.CreateFromRule(
            ActivityId.New(),
            activityRuleId,
            new ActivityDetailsSpecification(
                "test_activity_title",
                null));
        
        // Assert
        result.ParentActivityRuleId.ShouldBe(activityRuleId);
    }
    
    #endregion
    #region AddStage
    
    [Fact]
    public void GivenUniqueTitleAsFirstStage_WhenAddStage_ThenAddsNewStageWithIndexOne()
    {
        // Arrange
        var activity = ActivityFakeDataFactory
            .Get();
        
        const string title = "test_title";
        var stageId = StageId.New();
        
        // Act
        activity.AddStage(
            stageId,
            title);
        
        // Assert
        var stage = activity.Stages.Single();
        stage.Id.ShouldBe(stageId);
        stage.Title.Value.ShouldBe(title);
        stage.Index.Value.ShouldBe(1);
    } 
    
    [Fact]
    public void GivenUniqueTitleAsNextStage_WhenAddStage_ThenAddsNewStageWithNextIndex()
    {
        // Arrange
        var activity = ActivityFakeDataFactory
            .Get()
            .WithStages();
        
        const string title = "test_title";
        var stageId = StageId.New();
        
        // Act
        activity.AddStage(
            stageId,
            title);
        
        // Assert
        var stage = activity.Stages.Single(x => x.Id == stageId);
        stage.Id.ShouldBe(stageId);
        stage.Title.Value.ShouldBe(title);
        stage.Index.Value.ShouldBe(2);
    } 
    
    // [Fact]
    // public void GivenEmptyStages_WhenAddStage_ThenReturnStageWithFirstIndex()
    // {
    //     // Arrange
    //     var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
    //         null), null, null);
    //     
    //     // Act
    //     var stage = activity.AddStage("Test_title");
    //     
    //     // Assert
    //     stage.Index.Value.ShouldBe(1);
    // } 
    //
    // [Fact]
    // public void GivenExistingStages_WhenAddStage_ThenReturnStageWithNextIndex()
    // {
    //     // Arrange
    //     var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
    //         null), null, null);
    //     
    //     _ = activity.AddStage("Test_title");
    //     
    //     // Act
    //     var stage = activity.AddStage("Test_title_second");
    //     
    //     // Assert
    //     stage.Index.Value.ShouldBe(2);
    // } 
    //
    // [Fact]
    // public void GivenAlreadyRegisteredStageTitle_WhenAddStage_ThenThrowDomainExceptionWithCodeActivityRuleStagesStageTitleMustBeUnique()
    // {
    //     // Arrange
    //     var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title",
    //         null), null, null);
    //     const string title = "test_stage_title";
    //     
    //     activity.AddStage(title);
    //     
    //     // Act
    //     var exception = Record.Exception(() => activity.AddStage(title));
    //     
    //     // Assert
    //     exception.ShouldBeOfType<DomainException>();
    //     ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.StageTitleMustBeUnique");
    // } 
    //
    #endregion
}