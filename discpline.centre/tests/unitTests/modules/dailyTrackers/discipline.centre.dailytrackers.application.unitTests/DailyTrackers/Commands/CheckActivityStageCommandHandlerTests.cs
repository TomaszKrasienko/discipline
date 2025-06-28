using discipline.centre.dailytrackers.application.DailyTrackers.Commands;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.dailytrackers.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unitTests.DailyTrackers.Commands;

public sealed class CheckActivityStageCommandHandlerTests
{
    private Task Act(CheckActivityStageCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task Handle_ShouldUpdateDailyTrackerWithChangedActivityStage_WhenDailyTrackerExists()
    {
        //arrange
        var dailyTracker = DailyTrackerFakeDataFactory.Get(
            ActivityFakeDataFactory.Get(false, false, [StageFakeDataFactory.Get()]));
        var activity = dailyTracker.Activities.Single();
        var stage = activity.Stages!.Single();

        var command = new CheckActivityStageCommand(dailyTracker.UserId, dailyTracker.Id, activity.Id, stage.Id);
        
        _readWriteDailyTrackerRepository
            .GetDailyTrackerByIdAsync(command.UserId, command.DailyTrackerId, CancellationToken.None)
            .Returns(dailyTracker);
        
        //act
        await Act(command);
        
        //assert
        stage.IsChecked.Value.ShouldBeTrue();
        
        await _readWriteDailyTrackerRepository
            .Received(1)
            .UpdateAsync(dailyTracker, CancellationToken.None);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenDailyTrackerNotFound()
    {
        //arrange
        var command = new CheckActivityStageCommand(UserId.New(), DailyTrackerId.New(), ActivityId.New(),
            StageId.New());
        
        //act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
    }
    
    #region arrange
    private readonly IReadWriteDailyTrackerRepository _readWriteDailyTrackerRepository;
    private readonly CheckActivityStageCommandHandler _handler;

    public CheckActivityStageCommandHandlerTests()
    {
        _readWriteDailyTrackerRepository = Substitute.For<IReadWriteDailyTrackerRepository>();
        _handler = new CheckActivityStageCommandHandler(_readWriteDailyTrackerRepository);
    }
    #endregion
}