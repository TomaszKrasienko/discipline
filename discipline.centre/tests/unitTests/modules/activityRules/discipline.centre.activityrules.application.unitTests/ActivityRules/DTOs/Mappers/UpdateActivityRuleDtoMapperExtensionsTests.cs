using discipline.centre.activity_rules.tests.shared_kernel.Application;
using discipline.centre.activity_rules.tests.shared_kernel.DataValidators;
using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.DTOs.Mappers;

public sealed class UpdateActivityRuleDtoMapperExtensionsTests
{
    [Fact]
    public void GivenCreateActivityRuleRequestDtoWithoutSelectedDays_WhenAsCommand_ShouldReturnCreateActivityRuleCommand()
    {
        //arrange
        var dto = ActivityRuleRequestDtoFakeDataFactory.Get();
        var activityRuleId = ActivityRuleId.New();
        var accountId = AccountId.New();
        
        //act
        var result = dto.ToCreateCommand(accountId, activityRuleId);
        
        //assert
        result.Id.ShouldBe(activityRuleId);
        result.AccountId.ShouldBe(accountId);
        result.Details.Title.ShouldBe(dto.Details.Title);
        result.Details.Note.ShouldBe(dto.Details.Note);
        result.Mode.Mode.Value.ShouldBe(dto.Mode.Mode);
        result.Mode.Days.ShouldBeNull();
    }
    
    [Fact]
    public void GivenCreateActivityRuleRequestDtoWithSelectedDays_WhenAsCommand_ShouldReturnCreateActivityRuleCommand()
    {
        //arrange
        var dto = ActivityRuleRequestDtoFakeDataFactory.Get()
            .WithCustomMode();
        var activityRuleId = ActivityRuleId.New();
        var accountId = AccountId.New();
        
        //act
        var result = dto.ToCreateCommand(accountId, activityRuleId);
        
        //assert
        result.Id.ShouldBe(activityRuleId);
        result.AccountId.ShouldBe(accountId);
        result.Details.Title.ShouldBe(dto.Details.Title);
        result.Details.Note.ShouldBe(dto.Details.Note);
        result.Mode.Mode.Value.ShouldBe(dto.Mode.Mode);
        result.Mode.Days!.ToList().IsEqual(dto.Mode.Days).ShouldBeTrue();
    }
}