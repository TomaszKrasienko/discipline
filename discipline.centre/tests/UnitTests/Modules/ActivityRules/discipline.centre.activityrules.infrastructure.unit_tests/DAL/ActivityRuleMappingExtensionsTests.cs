using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.infrastructure.unit_tests.DAL;

public sealed class ActivityRuleMappingExtensionsTests
{
    [Fact]
    public void GivenActivityRuleWithoutSelectedDays_WhenAsDocument_ThenReturnActivityRuleDocumentWithNullSelectedDays()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        
        // Act
        var result = activityRule.AsDocument();
        
        // Assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        result.SelectedMode.DaysOfWeek.ShouldBeNull();
    }
    
    [Fact]
    public void GivenActivityRuleWithSelectedDays_WhenAsDocument_ShouldReturnActivityRuleDocument()
    {
        // Arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRule = ActivityRuleFakeDataFactory.Get(true, selectedDays.ToHashSet());
        
        // Act
        var result = activityRule.AsDocument();
        
        // Assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        result.SelectedMode.DaysOfWeek.ShouldBeEquivalentTo(selectedDays);
    }

    [Fact]
    public void GivenActivityRuleWithStage_WhenAsDocument_ThenActivityRuleShouldHaveStage()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get().WithStage();
        var stage = activityRule.Stages.Single();
        
        // Act
        var result = activityRule.AsDocument();
        
        // Assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        
        var stageDocument = result.Stages.Single();
        stageDocument.StageId.ShouldBe(stage.Id.Value.ToString());
        stageDocument.Title.ShouldBe(stage.Title.Value);
        stageDocument.Index.ShouldBe(stage.Index.Value);
    }
}