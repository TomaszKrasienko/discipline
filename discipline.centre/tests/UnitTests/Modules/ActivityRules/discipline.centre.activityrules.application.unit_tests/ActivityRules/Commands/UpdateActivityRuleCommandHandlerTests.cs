using discipline.centre.activityrules.application.ActivityRules.Commands;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Repositories;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.Commands;

public sealed class UpdateActivityRuleCommandHandlerTests
{
    private Task Act(UpdateActivityRuleCommand command) => _handler.HandleAsync(command, default);
    
    [Fact]
    public async Task HandleAsync_GivenValidData_ShouldUpdateByRepository(UpdateActivityRuleCommand command)
    {
        //arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();

        _readWriteActivityRuleRepository
            .GetByIdAsync(activityRule.Id, activityRule.UserId)
            .Returns(activityRule);
        
        //act
        await Record.ExceptionAsync(async () => await Act(command with { Id = activityRule.Id, UserId = activityRule.UserId}));
        
        //assert
        await _readWriteActivityRuleRepository
            .Received(1)
            .UpdateAsync(Arg.Is<ActivityRule>(arg 
                => arg.Details.Title == command.Details.Title
                && arg.Details.Note == command.Details.Note
                && arg.Mode == command.Mode
                && command.SelectedDays == null || arg.SelectedDays!.Values!.Select(x => (int)x).SequenceEqual(command.SelectedDays!)));
    }
    
    [Fact]
    public async Task GivenNotExistingActivityRule_WhenHandleAsync_ThenThrowNotFoundException()
    {
        //arrange
        var command = new UpdateActivityRuleCommand(UserId.New(), ActivityRuleId.New(), 
            new ActivityRuleDetailsSpecification("test_title", "test_note"),
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));

        _readWriteActivityRuleRepository
            .GetByIdAsync(command.Id, command.UserId)
            .ReturnsNull();
        
        //act
        var exception = await Record.ExceptionAsync(async () => await Act(command));
        
        //assert
        exception.ShouldBeOfType<NotFoundException>();
    }
    
    #region arrange;
    private readonly IReadWriteActivityRuleRepository _readWriteActivityRuleRepository;
    private readonly ICommandHandler<UpdateActivityRuleCommand> _handler;

    public UpdateActivityRuleCommandHandlerTests()
    { 
        _readWriteActivityRuleRepository = Substitute.For<IReadWriteActivityRuleRepository>();
        _handler = new UpdateActivityRuleCommandHandler(_readWriteActivityRuleRepository);
    }
    #endregion
}