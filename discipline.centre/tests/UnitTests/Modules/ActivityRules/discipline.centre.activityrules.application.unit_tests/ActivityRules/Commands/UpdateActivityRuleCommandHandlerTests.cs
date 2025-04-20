using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Events;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands;

public sealed class UpdateActivityRuleCommandHandlerTests
{
    private Task Act(UpdateActivityRuleCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenUniqueTitleAndExistingActivityRule_WhenHandleAsync_ShouldPublishIntegrationEvent()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var activityRule = ActivityRuleFakeDataFactory.Get();
        
        var command = new UpdateActivityRuleCommand(UserId.New(), activityRule.Id, 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.Id, command.UserId, cancellationToken)
            .Returns(activityRule);

        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.UserId, cancellationToken)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _eventProcessor
            .Received(1)
            .PublishAsync(Arg.Any<IEvent>());
    }
    
    [Fact]
    public async Task GivenUniqueTitleAndExistingActivityRule_WhenHandleAsync_ShouldUpdateActivityRule()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var activityRule = ActivityRuleFakeDataFactory.Get();
        
        var command = new UpdateActivityRuleCommand(UserId.New(), activityRule.Id, 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.Id, command.UserId, cancellationToken)
            .Returns(activityRule);

        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.UserId, cancellationToken)
            .Returns(false);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .UpdateAsync(Arg.Is<ActivityRule>(arg
                => arg.UserId == command.UserId
                   && arg.Id == command.Id
                   && arg.Details.Title == command.Details.Title
                   && arg.Details.Note == command.Details.Note
                   && arg.Mode.Mode == command.Mode.Mode), cancellationToken);
    }
    
    [Fact]
    public async Task GivenNotUniqueTitle_WhenHandleAsync_ShouldThrowNotUniqueExceptionWithCodeUpdateActivityRuleNotUniqueTitle()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        var activityRule = ActivityRuleFakeDataFactory.Get();
        
        var command = new UpdateActivityRuleCommand(UserId.New(), activityRule.Id, 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.Id, command.UserId, cancellationToken)
            .Returns(activityRule);

        _readWriteActivityRuleRepository
            .ExistsAsync(command.Details.Title, command.UserId, cancellationToken)
            .Returns(true);
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotUniqueException>();
        ((NotFoundException)exception).Code.ShouldBe("UpdateActivityRule.NotUniqueTitle");
    }
    
    [Fact]
    public async Task GivenNotExistingActivityRule_WhenHandleAsync_ThenThrowNotFoundExceptionWithCodeUpdateActivityRuleActivityRuleNotFound()
    {
        // Arrange
        var command = new UpdateActivityRuleCommand(UserId.New(), ActivityRuleId.New(), 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.Id, command.UserId, CancellationToken.None)
            .ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("UpdateActivityRule.ActivityRuleNotFound");
    }
    
    #region Arrange;
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly IEventProcessor _eventProcessor;
    private readonly ICommandHandler<UpdateActivityRuleCommand> _handler;

    public UpdateActivityRuleCommandHandlerTests()
    { 
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _eventProcessor = Substitute.For<IEventProcessor>();
        _handler = new UpdateActivityRuleCommandHandler(_readWriteActivityRuleRepository,
            _eventProcessor);
    }
    #endregion
}