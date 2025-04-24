using discipline.centre.activityrules.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

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
    public void GivenExistingStage_WhenRemoveStage_ThenDoesNothing()
}