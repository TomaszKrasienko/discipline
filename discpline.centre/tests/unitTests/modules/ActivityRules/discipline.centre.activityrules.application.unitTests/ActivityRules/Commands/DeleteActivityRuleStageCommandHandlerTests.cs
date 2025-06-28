using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.Commands;

public sealed class DeleteActivityRuleStageCommandHandlerTests
{
    private Task Act(DeleteActivityRuleStageCommand command) => _handler.HandleAsync(command, CancellationToken.None);
    
    [Fact]
    public async Task GivenNotExistingStage_WhenHandleAsync_ThenNotUpdateActivityRule()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get().WithStage();
        
        var command = new DeleteActivityRuleStageCommand(activityRule.UserId,
            activityRule.Id, StageId.New());

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.UserId, CancellationToken.None)
            .Returns(activityRule);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .UpdateAsync(Arg.Any<ActivityRule>(), CancellationToken.None);
    }

    [Fact]
    public async Task GivenExistingStage_WhenHandleAsync_ThenUpdateActivityRule()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get().WithStage();
        
        var command = new DeleteActivityRuleStageCommand(activityRule.UserId,
            activityRule.Id, activityRule.Stages.Single().Id);

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.UserId, CancellationToken.None)
            .Returns(activityRule);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .UpdateAsync(activityRule, CancellationToken.None);
    }
    
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly DeleteActivityRuleStageCommandHandler _handler;

    public DeleteActivityRuleStageCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _handler = new DeleteActivityRuleStageCommandHandler(_readWriteActivityRuleRepository);
    }
}