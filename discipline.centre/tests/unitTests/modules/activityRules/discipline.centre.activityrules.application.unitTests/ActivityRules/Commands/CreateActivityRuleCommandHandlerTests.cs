using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;
using discipline.libs.events.abstractions;
using NSubstitute;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.Commands;

public sealed class CreateActivityRuleCommandHandlerTests
{
    private Task Act(CreateActivityRuleCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenCorrectlyAddedActivityRule_WhenHandleAsync_ThenPublishesIntegrationEvent()
    {
        // Arrange
        var command = new CreateActivityRuleCommand(
            AccountId.New(),
            ActivityRuleId.New(), 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));
        
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.AccountId, CancellationToken.None)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _eventProcessor
            .Received(1)
            .PublishAsync(
                CancellationToken.None,
                Arg.Any<IEvent>());
    }
    
    [Fact]
    public async Task GivenUniqueTitle_WhenHandleAsync_ThenAddsNewActivityRule()
    {
        // Arrange
        var command = new CreateActivityRuleCommand(
            AccountId.New(),
            ActivityRuleId.New(), 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));
        
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.AccountId, CancellationToken.None)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .AddAsync(Arg.Is<ActivityRule>(arg
                => arg.AccountId == command.AccountId
                   && arg.Id == command.Id
                   && arg.Details.Title == command.Details.Title
                   && arg.Details.Note == command.Details.Note
                   && arg.Mode.Mode == command.Mode.Mode));
    }
    
    [Fact]
    public async Task GivenAlreadyRegisteredTitle_WhenHandleAsync_ThenThrowsNotUniqueExceptionWithCodeCreateActivityRuleNotUniqueTitle()
    {
        // Arrange
        var command = new CreateActivityRuleCommand(
            AccountId.New(),
            ActivityRuleId.New(), 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));
        
        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.AccountId, CancellationToken.None)
            .Returns(true);

        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotUniqueException>();
        ((NotUniqueException)exception).Code.ShouldBe("CreateActivityRule.NotUniqueTitle");
    }
    
    #region Arrange
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<CreateActivityRuleCommand> _handler;

    public CreateActivityRuleCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _handler = new CreateActivityRuleCommandHandler(_readWriteActivityRuleRepository,
            _eventProcessor);
    }
    #endregion
}