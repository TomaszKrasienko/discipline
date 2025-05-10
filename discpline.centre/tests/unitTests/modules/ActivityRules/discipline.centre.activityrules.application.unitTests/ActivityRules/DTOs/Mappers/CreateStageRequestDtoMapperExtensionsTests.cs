using discipline.centre.activityrules.application.ActivityRules.DTOs.Mappers;
using discipline.centre.activityrules.tests.sharedkernel.Application;
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
        var userId = UserId.New();
        var activityRuleId = ActivityRuleId.New();
        var stageId = StageId.New();
        
        // Act
        var result = request.MapAsCommand(userId, activityRuleId, stageId);
        
        // Assert
        result.UserId.ShouldBe(userId);
        result.ActivityRuleId.ShouldBe(activityRuleId);
        result.StageId.ShouldBe(stageId);
        result.Title.ShouldBe(request.Title);
        result.Index.ShouldBe(request.Index);
    }
}