using discipline.centre.activityrules.application.ActivityRules.DTOs;
using discipline.centre.activityrules.tests.sharedkernel.Application;
using discipline.centre.activityrules.tests.sharedkernel.DataValidators;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.DTOs.Mappers;
public sealed class CreateActivityRuleDtoMapperExtensionsTests
{
    [Fact]
    public void GivenCreateActivityRuleRequestDtoWithoutSelectedDays_WhenAsCommand_ShouldReturnCreateActivityRuleCommand()
    {
        //arrange
        var dto = ActivityRuleRequestDtoFakeDataFactory.Get();
        var activityRuleId = ActivityRuleId.New();
        var accountId = AccountId.New();
        
        //act
        var result = dto.MapAsCommand(accountId, activityRuleId);
        
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
        var result = dto.MapAsCommand(accountId, activityRuleId);
        
        //assert
        result.Id.ShouldBe(activityRuleId);
        result.AccountId.ShouldBe(accountId);
        result.Details.Title.ShouldBe(dto.Details.Title);
        result.Details.Note.ShouldBe(dto.Details.Note);
        result.Mode.Mode.Value.ShouldBe(dto.Mode.Mode);
        result.Mode.Days!.ToList().IsEqual(dto.Mode.Days).ShouldBeTrue();
    }
}