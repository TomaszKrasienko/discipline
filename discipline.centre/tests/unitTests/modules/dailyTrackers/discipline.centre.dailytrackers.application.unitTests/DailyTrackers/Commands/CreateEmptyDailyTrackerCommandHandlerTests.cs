using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unitTests.DailyTrackers.Commands;

public sealed class CreateEmptyDailyTrackerCommandHandlerTests
{
    private Task Act(CreateEmptyDailyTrackerCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenNotExistingDailyTrackerForAccount_WhenCreate_ThenSavesNewEmptyDailyTracker()
    {
        // Arrange
        var command = new CreateEmptyDailyTrackerCommand(
            DailyTrackerId.New(),
            DateOnly.FromDateTime(DateTime.UtcNow),
            AccountId.New());
        
        _dailyTrackerRepository
            .ExistsAsync(
                command.AccountId,
                command.Day,
                CancellationToken.None)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _dailyTrackerRepository
            .Received(1)
            .AddAsync(
                Arg.Is<DailyTracker>(arg =>
                arg.Id == command.Id &&
                arg.Day == command.Day &&
                arg.AccountId == command.AccountId),
                CancellationToken.None);
    }
    
    [Fact]
    public async Task GivenExistingDailyTrackerForAccount_WhenCreate_ThenThrowsNotUniqueExceptionWithCode_CreateEmptyDailyTracker_DailyTrackerNotUnique()
    {
        // Arrange
        var command = new CreateEmptyDailyTrackerCommand(
            DailyTrackerId.New(),
            DateOnly.FromDateTime(DateTime.UtcNow),
            AccountId.New());
        
        _dailyTrackerRepository
            .ExistsAsync(
                command.AccountId,
                command.Day,
                CancellationToken.None)
            .Returns(true);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotUniqueException>();
        ((NotUniqueException)exception).Code.ShouldBe("CreateEmptyDailyTracker.DailyTrackerNotUnique");
    }
    
    private readonly IWriteDailyTrackerRepository _dailyTrackerRepository;
    private readonly CreateEmptyDailyTrackerCommandHandler _handler;

    public CreateEmptyDailyTrackerCommandHandlerTests()
    {
        _dailyTrackerRepository = Substitute.For<IWriteDailyTrackerRepository>();
        _handler = new CreateEmptyDailyTrackerCommandHandler(_dailyTrackerRepository);
    }
}