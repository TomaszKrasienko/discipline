using discipline.centre.daily_trackers.tests.shared_kernel.Domain;
using discipline.centre.dailytrackers.domain.Specifications;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.domain.unitTests.DailyTrackerTests;

public sealed class DeleteActivityTests
{
    [Fact]
    public void GivenExistingActivityInDailyTracker_WhenDeleteActivityIsCalled_ThenShouldRemoveActivityFromDailyTracker()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get();
        dailyTracker.AddActivity(activity.Id, new ActivityDetailsSpecification(activity.Details.Title, activity.Details.Note),
            activity.ParentActivityRuleId, []);
        
        // Act
        _ = dailyTracker.DeleteActivity(activity.Id);
        
        // Assert
        dailyTracker.Activities.Any(x => x.Id == activity.Id).ShouldBeFalse();
    }
    
    [Fact]
    public void GivenExistingActivityInDailyTracker_WhenDeleteActivityIsCalled_ThenShouldReturnTrue()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get();
        dailyTracker.AddActivity(activity.Id, new ActivityDetailsSpecification(activity.Details.Title, activity.Details.Note),
            activity.ParentActivityRuleId, []);
        
        // Act
        var result = dailyTracker.DeleteActivity(activity.Id);
        
        // Assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void GivenNotExistingActivityInDailyTracker_WhenDeleteActivityIsCalled_ThenShouldReturnFalse()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get();
        
        // Act
        var result = dailyTracker.DeleteActivity(activity.Id);
        
        // Assert
        result.ShouldBeFalse();
    }
}