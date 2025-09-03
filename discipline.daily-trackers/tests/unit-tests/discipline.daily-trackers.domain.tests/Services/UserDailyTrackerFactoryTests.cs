using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Services;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.daily_trackers.tests.shared_kernel.Domain;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.Services;

public sealed class UserDailyTrackerFactoryTests
{
    [Fact]
    public async Task GivenNotExistingPriorUserDailyTracker_WhenCreateWithActivity_ThenSavesUserDailyTrackerWithoutPriorUserDailyTrackerId()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = AccountId.New();
        var day = Day.Create(DateOnly.FromDateTime(DateTime.Now));
        var activityId = ActivityId.New();
        var details = new ActivityDetailsSpecification(
            "test_title",
            "test_note");
        var activityRuleId = ActivityRuleId.New();

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, day.GetPriorDay(), cancellationToken)
            .ReturnsNull();
        
        // Act
        _ = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            activityId,
            details,
            activityRuleId,
            cancellationToken);
        
        // Assert
        await _userDailyTrackerRepository
            .Received(1)
            .AddAsync(Arg.Is<UserDailyTracker>(arg
                => arg.Id == dailyTrackerId
                   && arg.AccountId == accountId
                   && arg.Activities.Any(x => x.Id == activityId)
                   && arg.Day == day
                   && arg.Next == null
                   && arg.Prior == null), cancellationToken);

        await _userDailyTrackerRepository
            .Received(0)
            .UpdateAsync(Arg.Any<UserDailyTracker>(), cancellationToken);
    }
    
    [Fact]
    public async Task GivenNotExistingPriorUserDailyTracker_WhenCreateWithActivity_ThenReturnsUserDailyTrackerWithoutPriorUserDailyTrackerId()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = AccountId.New();
        var day = Day.Create(DateOnly.FromDateTime(DateTime.Now));
        var activityId = ActivityId.New();
        var details = new ActivityDetailsSpecification(
            "test_title",
            "test_note");
        var activityRuleId = ActivityRuleId.New();

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, day.GetPriorDay(), cancellationToken)
            .ReturnsNull();
        
        // Act
        var userDailyTracker = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            activityId,
            details,
            activityRuleId,
            cancellationToken);
        
        // Assert
        userDailyTracker.Id.ShouldBe(dailyTrackerId);
        userDailyTracker.AccountId.ShouldBe(accountId);
        userDailyTracker.Activities.Any(x => x.Id == activityId).ShouldBeTrue();
        userDailyTracker.Day.ShouldBe(day);
        userDailyTracker.Next.ShouldBeNull();
        userDailyTracker.Prior.ShouldBeNull();
    }

    [Fact]
    public async Task GivenExistingPriorUserDailyTracker_WhenCreateWithActivity_ThenSavesUserDailyTrackerWithPriorUserDailyTrackerIdAndUpdatesPriorUserDailyTracker()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var userDailyTracker = UserDailyTrackerFakeDataFactory.Get();
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = userDailyTracker.AccountId;
        var day = userDailyTracker.Day.Value.AddDays(1);
        var activityId = ActivityId.New();
        var details = new ActivityDetailsSpecification(
            "test_title",
            "test_note");
        var activityRuleId = ActivityRuleId.New();

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, userDailyTracker.Day, cancellationToken)
            .Returns(userDailyTracker);
        
        // Act
        _ = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            activityId,
            details,
            activityRuleId,
            cancellationToken);
        
        // Assert
        await _userDailyTrackerRepository
            .Received(1)
            .AddAsync(Arg.Is<UserDailyTracker>(arg
                => arg.Id == dailyTrackerId
                   && arg.AccountId == accountId
                   && arg.Activities.Any(x => x.Id == activityId)
                   && arg.Day == day
                   && arg.Next == null
                   && arg.Prior == userDailyTracker.Id), cancellationToken);

        await _userDailyTrackerRepository
            .Received(1)
            .UpdateAsync(userDailyTracker, cancellationToken);
    }
    
    [Fact]
    public async Task GivenExistingPriorUserDailyTracker_WhenCreateWithActivity_ThenSetsNextUseDailyTrackerIdInPriorUserDailyTracker()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var userDailyTracker = UserDailyTrackerFakeDataFactory.Get();
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = userDailyTracker.AccountId;
        var day = userDailyTracker.Day.Value.AddDays(1);
        var activityId = ActivityId.New();
        var details = new ActivityDetailsSpecification(
            "test_title",
            "test_note");
        var activityRuleId = ActivityRuleId.New();

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, userDailyTracker.Day, cancellationToken)
            .Returns(userDailyTracker);
        
        // Act
        _ = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            activityId,
            details,
            activityRuleId,
            cancellationToken);
        
        // Assert
        userDailyTracker.Next.ShouldBe(dailyTrackerId);
    }
    
        [Fact]
    public async Task GivenNotExistingPriorUserDailyTracker_WhenCreate_ThenSavesUserDailyTrackerWithoutPriorUserDailyTrackerId()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = AccountId.New();
        var day = Day.Create(DateOnly.FromDateTime(DateTime.Now));

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, day.GetPriorDay(), cancellationToken)
            .ReturnsNull();
        
        // Act
        _ = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            cancellationToken);
        
        // Assert
        await _userDailyTrackerRepository
            .Received(1)
            .AddAsync(Arg.Is<UserDailyTracker>(arg
                => arg.Id == dailyTrackerId
                   && arg.AccountId == accountId
                   && arg.Day == day
                   && arg.Next == null
                   && arg.Prior == null), cancellationToken);

        await _userDailyTrackerRepository
            .Received(0)
            .UpdateAsync(Arg.Any<UserDailyTracker>(), cancellationToken);
    }
    
    [Fact]
    public async Task GivenNotExistingPriorUserDailyTracker_WhenCreate_ThenReturnsUserDailyTrackerWithoutPriorUserDailyTrackerId()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = AccountId.New();
        var day = Day.Create(DateOnly.FromDateTime(DateTime.Now));

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, day.GetPriorDay(), cancellationToken)
            .ReturnsNull();
        
        // Act
        var userDailyTracker = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            cancellationToken);
        
        // Assert
        userDailyTracker.Id.ShouldBe(dailyTrackerId);
        userDailyTracker.AccountId.ShouldBe(accountId);
        userDailyTracker.Day.ShouldBe(day);
        userDailyTracker.Next.ShouldBeNull();
        userDailyTracker.Prior.ShouldBeNull();
    }

    [Fact]
    public async Task GivenExistingPriorUserDailyTracker_WhenCreate_ThenSavesUserDailyTrackerWithPriorUserDailyTrackerIdAndUpdatesPriorUserDailyTracker()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var userDailyTracker = UserDailyTrackerFakeDataFactory.Get();
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = userDailyTracker.AccountId;
        var day = userDailyTracker.Day.Value.AddDays(1);

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, userDailyTracker.Day, cancellationToken)
            .Returns(userDailyTracker);
        
        // Act
        _ = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            cancellationToken);
        
        // Assert
        await _userDailyTrackerRepository
            .Received(1)
            .AddAsync(Arg.Is<UserDailyTracker>(arg
                => arg.Id == dailyTrackerId
                   && arg.AccountId == accountId
                   && arg.Day == day
                   && arg.Next == null
                   && arg.Prior == userDailyTracker.Id), cancellationToken);

        await _userDailyTrackerRepository
            .Received(1)
            .UpdateAsync(userDailyTracker, cancellationToken);
    }
    
    [Fact]
    public async Task GivenExistingPriorUserDailyTracker_WhenCreate_ThenSetsNextUseDailyTrackerIdInPriorUserDailyTracker()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var userDailyTracker = UserDailyTrackerFakeDataFactory.Get();
        
        var dailyTrackerId = DailyTrackerId.New();
        var accountId = userDailyTracker.AccountId;
        var day = userDailyTracker.Day.Value.AddDays(1);
        var activityId = ActivityId.New();
        var details = new ActivityDetailsSpecification(
            "test_title",
            "test_note");
        var activityRuleId = ActivityRuleId.New();

        _userDailyTrackerRepository
            .GetByDayAsync(accountId, userDailyTracker.Day, cancellationToken)
            .Returns(userDailyTracker);
        
        // Act
        _ = await _userDailyTrackerFactory.Create(
            dailyTrackerId,
            accountId,
            day,
            activityId,
            details,
            activityRuleId,
            cancellationToken);
        
        // Assert
        userDailyTracker.Next.ShouldBe(dailyTrackerId);
    }
    
    private readonly IReadWriteUserDailyTrackerRepository _userDailyTrackerRepository;
    private readonly IUserDailyTrackerFactory _userDailyTrackerFactory;

    public UserDailyTrackerFactoryTests()
    {
        _userDailyTrackerRepository = Substitute.For<IReadWriteUserDailyTrackerRepository>();
        _userDailyTrackerFactory = new UserDailyTrackerFactory(_userDailyTrackerRepository);
    }
}