using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unitTests.ActivityRulesTests;

public sealed class AddStageTests
{
    [Fact]
    public void GivenUniqueTitle_WhenAddStage_ShouldAddStageWithValidIndex()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        activityRule.AddStage(StageId.New(), "test_stage_1");
        var stageId = StageId.New();
        const string title = "test_stage_title2";
        
        // Act
        activityRule.AddStage(stageId, title);
        
        // Assert
        var stage = activityRule.Stages.SingleOrDefault(x 
            => x.Id == stageId 
            && x.Title == title);
        stage.ShouldNotBeNull();
        stage.Index.Value.ShouldBe(2);
    }

    [Fact]
    public void GivenFirstStage_WhenAddStage_ShouldAddStageWithFirstIndex()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var id = StageId.New();
        const string title = "test_stage_title";
        
        // Act
        activityRule.AddStage(id, title);
        
        // Assert
        var stage = activityRule.Stages.SingleOrDefault(x 
            => x.Id == id
            && x.Title == title);
        stage.ShouldNotBeNull();
        stage.Index.Value.ShouldBe(1);
    }

    [Fact]
    public void GivenNotUniqueTitle_WhenAddStage_ShouldThrowDomainExceptionWithCodeActivityRuleStagesStageTitleMustBeUnique()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        const string title = "test_stage_title";
        activityRule.AddStage(StageId.New(), title);
        
        // Act
        var exception = Record.Exception(() => activityRule.AddStage(StageId.New(), title));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.StageTitleMustBeUnique");
    }
}