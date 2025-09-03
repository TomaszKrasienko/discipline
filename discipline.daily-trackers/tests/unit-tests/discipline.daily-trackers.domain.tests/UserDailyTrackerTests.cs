using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.daily_trackers.tests.shared_kernel.Domain;
using Shouldly;

namespace discipline.daily_trackers.domain.tests;

public sealed class UserDailyTrackerTests
{
    #region Create
    
    [Fact]
    public void GivenValidArgumentsWithActivity_WhenCreate_ThenReturnsDailyTrackerWithValue()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = AccountId.New();
        var day = DateOnly.FromDateTime(DateTime.UtcNow);
        var activityId = ActivityId.New();
        var details = new ActivityDetailsSpecification("test_title_activity", "test_notes");
        var activityRuleId = ActivityRuleId.New();
        var priorUserDailyTrackerId = DailyTrackerId.New();
        
         // Act
         var result = UserDailyTracker.Create(
             dailyTrackerId,
             accountId,
             day,
             activityId,
             details,
             activityRuleId,
             priorUserDailyTrackerId);
     
         // Assert
         result.Id.ShouldBe(dailyTrackerId);
         result.AccountId.ShouldBe(accountId);
         result.Day.Value.ShouldBe(day);
         result.Prior.ShouldBe(priorUserDailyTrackerId);
         result.Activities.First().Id.ShouldBe(activityId);
         result.Activities.First().Details.Title.ShouldBe(details.Title);
         result.Activities.First().Details.Note.ShouldBe(details.Note);
         result.Activities.First().ParentActivityRuleId.ShouldBe(activityRuleId);
    }
    
    [Fact]
    public void GivenValidArguments_WhenCreate_ThenReturnsDailyTrackerWithValue()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = AccountId.New();
        var day = DateOnly.FromDateTime(DateTime.UtcNow);
        var priorUserDailyTrackerId = DailyTrackerId.New();
        
        // Act
        var result = UserDailyTracker.Create(
            dailyTrackerId,
            accountId,
            day,
            priorUserDailyTrackerId);
     
        // Assert
        result.Id.ShouldBe(dailyTrackerId);
        result.AccountId.ShouldBe(accountId);
        result.Day.Value.ShouldBe(day);
        result.Prior.ShouldBe(priorUserDailyTrackerId);
        result.Activities.Count.ShouldBe(0);
    }
    
    [Fact]
    public void GivenDefaultDayValue_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerDayDefault()
    {
        // Act
        var exception = Record.Exception(() => UserDailyTracker.Create(
            DailyTrackerId.New(),
            AccountId.New(),
            default,
            null));
     
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Day.Default");
    }
    
    #endregion
    #region AddActivity
    
    [Fact]
    public void GivenUniqueTitle_WhenAddActivity_ThenAddsActivity()
    {
        // Arrange
        var userDailyTracker = UserDailyTrackerFakeDataFactory
            .Get();
        
        var activityId = ActivityId.New();
        var activityDetails = new ActivityDetailsSpecification("test_title_activity", "test_notes");
        var activityRuleId = ActivityRuleId.New();
        
        // Act
        userDailyTracker.AddActivity(
            activityId, 
            activityDetails,
            activityRuleId);

        //assert
        var newActivity = userDailyTracker.Activities.Single(x => x.Id == activityId);
        newActivity.Details.Title.ShouldBe(activityDetails.Title);
        newActivity.Details.Note.ShouldBe(activityDetails.Note);
        newActivity.ParentActivityRuleId.ShouldBe(activityRuleId);
    }
    
    [Fact]
    public void GivenAlreadyExistingTitle_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerActivityTitleAlreadyExists()
    {
        // Arrange
        var userDailyTracker = UserDailyTrackerFakeDataFactory
            .Get();
        
        var activityId = ActivityId.New();
        var activityDetails = new ActivityDetailsSpecification("test_title_activity", "test_notes");
        var activityRuleId = ActivityRuleId.New();
        
        userDailyTracker.AddActivity(
            activityId, 
            activityDetails,
            activityRuleId);
        
        // Act
        var exception = Record.Exception(() => userDailyTracker.AddActivity(
            ActivityId.New(),
            new ActivityDetailsSpecification(activityDetails.Title, null),
            null));

        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Title.AlreadyExists");
    }
    
    #endregion
}