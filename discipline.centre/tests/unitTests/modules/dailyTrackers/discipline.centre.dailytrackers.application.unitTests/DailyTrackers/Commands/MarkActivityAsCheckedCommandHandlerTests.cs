using discipline.centre.daily_trackers.tests.shared_kernel.Domain;
using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unitTests.DailyTrackers.Commands;

public sealed class MarkActivityAsCheckedCommandHandlerTests
{
    private Task Act(MarkActivityAsCheckedCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenExistingDailyTrackerAndActivity_WhenHandleAsync_ThenShouldUpdateDailyTracker()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTracker.AccountId, dailyTracker.Id, CancellationToken.None)
            .Returns(dailyTracker);

        var command = new MarkActivityAsCheckedCommand(dailyTracker.AccountId, dailyTracker.Id, activity.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker, CancellationToken.None);
    }
    
    [Fact]
    public async Task GivenExistingDailyTrackerAndActivity_WhenHandleAsync_ThenShouldMarkActivityAsChecked()
    {
        // Arrange
        var activity = ActivityFakeDataFactory.Get();
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(dailyTracker.AccountId, dailyTracker.Id, CancellationToken.None)
            .Returns(dailyTracker);

        var command = new MarkActivityAsCheckedCommand(dailyTracker.AccountId, dailyTracker.Id, activity.Id);
        
        // Act
        await Act(command);
        
        // Assert
        dailyTracker.Activities.Single().IsChecked.Value.ShouldBeTrue();
    }
    
    [Fact]
    public async Task GivenNotExistingDailyTracker_WhenHandleAsync_ThenThrowNotFoundException()
    {
        // Arrange
        var dailyTrackerId = DailyTrackerId.New();
        var activityId = ActivityId.New();
        var accountId = AccountId.New();
        var command = new MarkActivityAsCheckedCommand(accountId, dailyTrackerId, activityId);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
    }
    
    #region arrange
    private readonly IWriteDailyTrackerRepository _readWriteDailyTrackerRepository;
    private readonly MarkActivityAsCheckedCommandHandler _handler;

    public MarkActivityAsCheckedCommandHandlerTests()
    {
        _readWriteDailyTrackerRepository = Substitute.For<IWriteDailyTrackerRepository>();
        _handler = new MarkActivityAsCheckedCommandHandler(_readWriteDailyTrackerRepository);
    }

    #endregion
}