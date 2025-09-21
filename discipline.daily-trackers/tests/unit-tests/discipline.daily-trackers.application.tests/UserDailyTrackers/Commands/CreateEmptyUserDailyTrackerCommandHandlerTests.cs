using discipline.daily_trackers.application.UserDailyTrackers.Commands;
using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Services;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.exceptions.Exceptions;
using NSubstitute;
using Shouldly;

namespace discipline.daily_trackers.application.tests.UserDailyTrackers.Commands;

public sealed class CreateEmptyUserDailyTrackerCommandHandlerTests
{
    private Task Act(CreateEmptyUserDailyTrackerCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenUniqueParameters_WhenHandleAsync_ShouldCreateNewUserDailyTrackerByFactory()
    {
        // Arrange
        var command = new CreateEmptyUserDailyTrackerCommand(
            DailyTrackerId.New(),
            AccountId.New(),
            DateOnly.FromDateTime(DateTimeOffset.UtcNow.DateTime));

        _userDailyTrackerRepository
            .DoesExistAsync(command.AccountId, command.Day, CancellationToken.None)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _userDailyTrackerFactory
            .Received(1)
            .Create(command.Id, command.AccountId, command.Day, CancellationToken.None);
    }

    [Fact]
    public async Task GivenNotUniqueParameters_WhenHandleAsync_ShouldThrowDisciplineNotUniqueExceptionWithCode_CreateEmptyUserDailyTracker_NotUnique()
    {
        // Arrange
        var command = new CreateEmptyUserDailyTrackerCommand(
            DailyTrackerId.New(),
            AccountId.New(),
            DateOnly.FromDateTime(DateTimeOffset.UtcNow.DateTime));

        _userDailyTrackerRepository
            .DoesExistAsync(command.AccountId, command.Day, CancellationToken.None)
            .Returns(true);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<DisciplineNotUniqueException>();
        ((DisciplineNotUniqueException)exception).Code.ShouldBe("CreateEmptyUserDailyTracker.NotUnique");
    }
    
    private readonly IUserDailyTrackerFactory _userDailyTrackerFactory;
    private readonly IReadWriteUserDailyTrackerRepository _userDailyTrackerRepository;
    private readonly CreateEmptyUserDailyTrackerCommandHandler _handler;

    public CreateEmptyUserDailyTrackerCommandHandlerTests()
    {
        _userDailyTrackerFactory = Substitute.For<IUserDailyTrackerFactory>();
        _userDailyTrackerRepository = Substitute.For<IReadWriteUserDailyTrackerRepository>();
        _handler = new CreateEmptyUserDailyTrackerCommandHandler(_userDailyTrackerFactory, _userDailyTrackerRepository);
    }
}