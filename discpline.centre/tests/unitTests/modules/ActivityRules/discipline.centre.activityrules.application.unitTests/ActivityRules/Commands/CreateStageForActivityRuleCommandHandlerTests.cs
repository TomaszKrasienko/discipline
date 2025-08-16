using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.Commands;

public sealed class CreateStageForActivityRuleCommandHandlerTests
{
    private Task Act(CreateStageForActivityRuleCommand command) => _handler.HandleAsync(command, CancellationToken.None);

    [Fact]
    public async Task GivenExistingActivityRule_WhenHandleAsync_ThenShouldUpdateActivityRule()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var command = new CreateStageForActivityRuleCommand(activityRule.AccountId,
            activityRule.Id,
            StageId.New(),
            "test_title",
            null);
        
        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.AccountId)
            .Returns(activityRule);
        
        // Act
        await Act(command);
        
        // Assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .UpdateAsync(activityRule, CancellationToken.None);
    }
    
    [Fact]
    public async Task GivenExistingActivityRule_WhenHandleAsync_ThenShouldAddStageToActivityRule()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var command = new CreateStageForActivityRuleCommand(activityRule.AccountId,
            activityRule.Id,
            StageId.New(),
            "test_title",
            null);
        
        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.AccountId)
            .Returns(activityRule);
        
        // Act
        await Act(command);
        
        // Assert
        activityRule
            .Stages.Any(x
                => x.Id == command.StageId && x.Title == command.Title).ShouldBeTrue();
    }
    
    [Fact]
    public async Task GivenNotExistingActivityRule_WhenHandleAsync_ThenThrowNotFoundExceptionWithCodeCreateStageForActivityRule_ActivityRuleNotFound()
    {
        // Arrange
        var command = new CreateStageForActivityRuleCommand(AccountId.New(),
            ActivityRuleId.New(),
            StageId.New(),
            "test_title",
            null);

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.ActivityRuleId, command.AccountId, CancellationToken.None)
            .ReturnsNull();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Act(command));
        
        // Assert
        exception.ShouldBeOfType<NotFoundException>();
        ((NotFoundException)exception).Code.ShouldBe("CreateStageForActivityRule.ActivityRuleNotFound");
    }
    
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly CreateStageForActivityRuleCommandHandler _handler;

    public CreateStageForActivityRuleCommandHandlerTests()
    {
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _handler = new CreateStageForActivityRuleCommandHandler(_readWriteActivityRuleRepository);
    }
}