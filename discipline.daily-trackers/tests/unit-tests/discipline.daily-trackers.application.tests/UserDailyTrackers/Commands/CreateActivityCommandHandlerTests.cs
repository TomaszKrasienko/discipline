using discipline.daily_trackers.application.UserDailyTrackers.Commands;
using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Services;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.daily_trackers.tests.shared_kernel.Domain;
using discipline.libs.cqrs.abstractions.Commands;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace discipline.daily_trackers.application.tests.UserDailyTrackers.Commands;

public sealed class CreateActivityCommandHandlerTests
{
    private Task Act(CreateActivityCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenNotExistingDailyTracker_WhenHandleAsync_ShouldAddDailyTrackerWithActivity()
    {
        // Arrange
        var command = new CreateActivityCommand(
            AccountId.New(), 
            ActivityId.New(), 
            new DateOnly(2025,1,1),
            new ActivityDetailsSpecification("new_test_activity", null));

        _readWriteUserDailyTrackerRepository
            .GetByDayAsync(command.AccountId, command.Day, CancellationToken.None)
            .ReturnsNull();

        // Act
        await Act(command);
        
        // Assert
        await _dailyTrackerFactory
            .Received(1)
            .Create(
                Arg.Any<DailyTrackerId>(),
                command.AccountId,
                command.Day,
                command.ActivityId,
                command.Details,
                null,
                CancellationToken.None);
    }
    
    [Fact]
    public async Task GivenExistingDailyTracker_HandleAsync_ShouldUpdateDailyTrackerWithActivity()
    {
        // Arrange
        var dailyTracker = UserDailyTrackerFakeDataFactory.Get();

        var command = new CreateActivityCommand(
            dailyTracker.AccountId, 
            ActivityId.New(), 
            dailyTracker.Day,
            new ActivityDetailsSpecification("new_test_activity", null));
        
        _readWriteUserDailyTrackerRepository
            .GetByDayAsync(dailyTracker.AccountId, command.Day, CancellationToken.None)
            .Returns(dailyTracker);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteUserDailyTrackerRepository
            .Received(1)
            .UpdateAsync(Arg.Is<UserDailyTracker>(x 
                => x.AccountId == command.AccountId
                && x.Day == command.Day
                && x.Activities.Any(act => act.Id == command.ActivityId)), CancellationToken.None);
    }
    
    #region Arrange
    private readonly IReadWriteUserDailyTrackerRepository _readWriteUserDailyTrackerRepository;
    private readonly IUserDailyTrackerFactory _dailyTrackerFactory;
    private readonly ICommandHandler<CreateActivityCommand> _handler;

    public CreateActivityCommandHandlerTests()
    {
        _readWriteUserDailyTrackerRepository = Substitute.For<IReadWriteUserDailyTrackerRepository>();
        _dailyTrackerFactory = Substitute.For<IUserDailyTrackerFactory>();
        _handler = new CreateActivityCommandHandler(
            _readWriteUserDailyTrackerRepository,
            _dailyTrackerFactory);
    }
    #endregion
}