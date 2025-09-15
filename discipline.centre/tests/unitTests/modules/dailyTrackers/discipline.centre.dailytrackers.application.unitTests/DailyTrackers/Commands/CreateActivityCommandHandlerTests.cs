using discipline.centre.daily_trackers.tests.shared_kernel.Domain;
using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unitTests.DailyTrackers.Commands;

public sealed class CreateActivityCommandHandlerTests
{
    private Task Act(CreateActivityCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityTitleAndNotExistingDailyTracker_ShouldAddDailyTracker()
    {
        //arrange
        var command = new CreateActivityCommand(AccountId.New(), ActivityId.New(), new DateOnly(2025,1,1),
        new ActivityDetailsSpecification("new_test_activity", null),
        null);

        _readWriteDailyTrackerRepository
            .GetDailyTrackerByDayAsync(command.AccountId, command.Day, CancellationToken.None)
            .ReturnsNull();

        //act
        await Act(command);
        
        //assert
        await _readWriteDailyTrackerRepository
            .Received(1)
            .AddAsync(Arg.Is<DailyTracker>(arg
                => arg.Day == command.Day
                   && arg.AccountId == command.AccountId
                   && arg.Activities.Count == 1
                   && arg.Activities.Any(x
                       => x.Id == command.ActivityId 
                       && x.Details.Title == command.Details.Title)
                   ), CancellationToken.None);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityTitleAndExistingDailyTracker_ShouldAddActivityToDailyTracker()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        var command = new CreateActivityCommand(dailyTracker.AccountId, ActivityId.New(), dailyTracker.Day,
            new ActivityDetailsSpecification("new_test_activity", null),
            null);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByDayAsync( dailyTracker.AccountId, command.Day, CancellationToken.None)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        dailyTracker.Activities
            .Any(x => x.Id == command.ActivityId).ShouldBeTrue();
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityTitleAndExistingDailyTracker_ShouldUpdateDailyTracker()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        var command = new CreateActivityCommand(dailyTracker.AccountId, ActivityId.New(), dailyTracker.Day,
            new ActivityDetailsSpecification("new_test_activity", null),
            null);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByDayAsync(dailyTracker.AccountId, command.Day, CancellationToken.None)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        await _readWriteDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker, CancellationToken.None);
    }
    
    [Fact]
    public async Task HandleAsync_GivenExistingActivityTitleForDay_ShouldThrowDomainException()
    {
        //arrange
        var activity = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_activity_title", null),
            null, null);
        var dailyTracker = DailyTrackerFakeDataFactory.Get(activity);

        var command = new CreateActivityCommand(dailyTracker.AccountId, ActivityId.New(), dailyTracker.Day,
            new ActivityDetailsSpecification(activity.Details.Title, null),
            null);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByDayAsync(dailyTracker.AccountId, command.Day, CancellationToken.None)
            .Returns(dailyTracker);
        
        //act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
    }
    
    #region arrange
    private readonly IWriteDailyTrackerRepository _readWriteDailyTrackerRepository;
    private readonly ICommandHandler<CreateActivityCommand> _handler;

    public CreateActivityCommandHandlerTests()
    {
        _readWriteDailyTrackerRepository = Substitute.For<IWriteDailyTrackerRepository>();
        _handler = new CreateActivityCommandHandler(_readWriteDailyTrackerRepository);
    }
    #endregion
}