using discipline.centre.activity_rules.tests.shared_kernel.Domain;
using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.application.ActivityRules.Events;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.Commands;

public sealed class DeleteActivityRuleCommandHandlerTests
{
    private Task Act(DeleteActivityRuleCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenExistingActivityRule_WhenHandleAsync_ThenShouldDeleteActivityRuleByRepository()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.AccountId)
            .Returns(activityRule);

        var command = new DeleteActivityRuleCommand(activityRule.AccountId, activityRule.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .DeleteAsync(activityRule);
    }
    
    [Fact]
    public async Task GivenExistingActivityRule_WhenHandleAsync_ThenShouldSendActivityRuleRemovedEvent()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.AccountId)
            .Returns(activityRule);

        var command = new DeleteActivityRuleCommand(activityRule.AccountId, activityRule.Id);
        
        // Act
        await Act(command);
        
        // Assert
        await _eventProcessor
            .Received(1)
            .PublishAsync(
                CancellationToken.None,
                Arg.Is<ActivityRuleDeleted>(arg
                => arg.UserId == activityRule.AccountId.Value
                && arg.ActivityRuleId == activityRule.Id.Value));
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ThenNotAttemptsToDeleteByRepository()
    {
        // Arrange
        var command = new DeleteActivityRuleCommand(AccountId.New(), ActivityRuleId.New());

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.AccountId)
            .ReturnsNull();
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .Received(0)
            .DeleteAsync(Arg.Any<ActivityRule>(), CancellationToken.None);
    }
    
    [Fact]
    public async Task HandleAsync_GivenNotExistingActivityRule_ThenNotAttemptsToSendActivityRuleRemovedEvent()
    {
        // Arrange
        var command = new DeleteActivityRuleCommand(AccountId.New(), ActivityRuleId.New());

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.AccountId)
            .ReturnsNull();
        
        // Act
        await Act(command);
        
        // Assert
        await _eventProcessor
            .Received(0)
            .PublishAsync(
                CancellationToken.None,
                Arg.Any<ActivityRuleDeleted>());
    }
    
    #region Arrange
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<DeleteActivityRuleCommand> _handler;
    
    public DeleteActivityRuleCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _handler = new DeleteActivityRuleCommandHandler(_readWriteActivityRuleRepository,
            _eventProcessor);
    }
    #endregion
}