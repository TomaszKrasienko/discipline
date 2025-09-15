using discipline.centre.activity_rules.tests.shared_kernel.DataValidators;
using discipline.centre.activity_rules.tests.shared_kernel.Domain;
using discipline.centre.activityrules.domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.infrastructure.unitTests.DAL;

public sealed class ActivityRuleMappingExtensionsTests
{
    [Fact]
    public void GivenActivityRuleWithoutSelectedDays_WhenAsDocument_ThenReturnActivityRuleDocumentWithNullSelectedDays()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        
        // Act
        var result = activityRule.ToDocument();
        
        // Assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.AccountId.ShouldBe(activityRule.AccountId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        result.SelectedMode.DaysOfWeek.ShouldBeNull();
    }
    
    [Fact]
    public void GivenActivityRuleWithSelectedDays_WhenAsDocument_ShouldReturnActivityRuleDocument()
    {
        // Arrange
        List<int> selectedDays = [1, 2, 3];
        var activityRule = ActivityRuleFakeDataFactory.Get(true, selectedDays.ToHashSet());
        
        // Act
        var result = activityRule.ToDocument();
        
        // Assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.AccountId.ShouldBe(activityRule.AccountId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        result.SelectedMode.DaysOfWeek!.ToList().IsEqual(selectedDays).ShouldBeTrue();
    }

    [Fact]
    public void GivenActivityRuleWithStage_WhenAsDocument_ThenActivityRuleShouldHaveStage()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get().WithStage();
        var stage = activityRule.Stages.Single();
        
        // Act
        var result = activityRule.ToDocument();
        
        // Assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.AccountId.ShouldBe(activityRule.AccountId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        
        var stageDocument = result.Stages.Single();
        stageDocument.StageId.ShouldBe(stage.Id.Value.ToString());
        stageDocument.Title.ShouldBe(stage.Title.Value);
        stageDocument.Index.ShouldBe(stage.Index.Value);
    }
}