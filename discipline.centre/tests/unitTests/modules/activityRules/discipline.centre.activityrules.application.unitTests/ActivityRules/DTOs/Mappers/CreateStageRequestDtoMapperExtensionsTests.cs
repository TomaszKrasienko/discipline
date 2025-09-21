using discipline.centre.activity_rules.tests.shared_kernel.Application;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Mappers;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.DTOs.Mappers;

public sealed class CreateStageRequestDtoMapperExtensionsTests
{
    [Fact]
    public void GivenCreateStageRequestDtoWithParameters_WhenMapAsCommand_ThenReturnCreateStageForActivityRuleCommand()
    {
        // Arrange
        var request = CreateStageRequestDtoFakeDataFactory.Get();
        var accountId = AccountId.New();
        var activityRuleId = ActivityRuleId.New();
        var stageId = StageId.New();
        
        // Act
        var result = request.MapAsCommand(accountId, activityRuleId, stageId);
        
        // Assert
        result.AccountId.ShouldBe(accountId);
        result.ActivityRuleId.ShouldBe(activityRuleId);
        result.StageId.ShouldBe(stageId);
        result.Title.ShouldBe(request.Title);
        result.Index.ShouldBe(request.Index);
    }
}