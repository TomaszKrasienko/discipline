using discipline.centre.activityrules.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unitTests.ActivityRulesTests;

public sealed class RemoveStageTests
{
    [Fact]
    public void GivenExistingStage_WhenRemoveStage_ThenRemovesStage()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get()
            .WithStage();

        var stage = activityRule.Stages.Single();
        
        // Act
        activityRule.RemoveStage(stage.Id);
        
        // Assert
        activityRule.Stages.Any(x => x.Id == stage.Id).ShouldBeFalse();
    }

    [Fact]
    public void GivenExistingStage_WhenRemoveStage_ThenShouldSetNewIndexOnExistingStages()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get()
            .WithStage()
            .WithStage()
            .WithStage();
        
        var stage = activityRule.Stages.First();
        
        // Act
        activityRule.RemoveStage(stage.Id);
        
        // Assert
        var existingStages = activityRule.Stages
            .OrderBy(x => x.Index.Value)
            .ToList();
        
        existingStages[0].Index.Value.ShouldBe(1);
        existingStages[1].Index.Value.ShouldBe(2);
    }
}