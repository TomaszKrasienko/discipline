using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.DataValidators;
using discipline.centre.activityrules.tests.sharedkernel.Infrastructure;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.infrastructure.unitTests.DAL;

public sealed class ActivityRuleDocumentMappingExtensionsTests
{
    [Fact]
    public void GivenActivityRuleDocumentWithoutSelectedDays_WhenAsEntity_ShouldReturnActivityRuleWithNullSelectedDays()
    {
        // Arrange
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get();
        
        // Act
        var entity = activityRuleDocument.AsEntity();
        
        // Assert
        entity.Id.ShouldBe(ActivityRuleId.Parse(activityRuleDocument.Id));
        entity.UserId.ShouldBe(UserId.Parse(activityRuleDocument.UserId));
        entity.Details.Title.ShouldBe(activityRuleDocument.Details.Title);
        entity.Details.Note.ShouldBe(activityRuleDocument.Details.Note);
        entity.Mode.Mode.Value.ShouldBe(activityRuleDocument.SelectedMode.Mode);
        entity.Mode.Days.ShouldBeNull();
        entity.Stages.ShouldBeEmpty();
    }
    
    [Fact]
    public void GivenActivityRuleDocumentWithSelectedDays_WhenAsEntity_ShouldReturnActivityRuleWithSelectedDays()
    {
        // Arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get(false, selectedDays.ToHashSet());
        
        // Act
        var entity = activityRuleDocument.AsEntity();
        
        // Assert
        entity.Id.ShouldBe(ActivityRuleId.Parse(activityRuleDocument.Id));
        entity.UserId.ShouldBe(UserId.Parse(activityRuleDocument.UserId));
        entity.Details.Title.ShouldBe(activityRuleDocument.Details.Title);
        entity.Details.Note.ShouldBe(activityRuleDocument.Details.Note);
        entity.Mode.Mode.Value.ShouldBe(activityRuleDocument.SelectedMode.Mode);
        entity.Mode.Days.IsEqual(selectedDays).ShouldBeTrue();
    }

    [Fact]
    public void GivenActivityRuleDocumentWithStages_WhenAsEntity_ShouldReturnActivityRuleWithStages()
    {
        // Arrange
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get().WithStage();
        var stageDocument = activityRuleDocument.Stages.Single();
        
        // Act
        var entity = activityRuleDocument.AsEntity();
        
        // Assert
        entity.Id.ShouldBe(ActivityRuleId.Parse(activityRuleDocument.Id));
        entity.UserId.ShouldBe(UserId.Parse(activityRuleDocument.UserId));
        entity.Details.Title.ShouldBe(activityRuleDocument.Details.Title);
        entity.Details.Note.ShouldBe(activityRuleDocument.Details.Note);
        entity.Mode.Mode.Value.ShouldBe(activityRuleDocument.SelectedMode.Mode);
        var stage = entity.Stages.Single();
        stage.Id.ToString().ShouldBe(stageDocument.StageId);
        stage.Title.Value.ShouldBe(stageDocument.Title);
        stage.Index.Value.ShouldBe(stageDocument.Index);
    }
    
    [Fact]
    public void GivenActivityRuleDocumentWithSelectedDaysAndStage_WhenAsResponseDto_ShouldReturnActivityRuleResponseDtoWithSelectedDaysAndStage()
    {
        // Arrange
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get()
            .WithStage();
        var stageDocument = activityRuleDocument.Stages.Single();
        
        // Act
        var dto = activityRuleDocument.AsResponseDto();
        
        // Assert
        dto.ActivityRuleId.ShouldBe(activityRuleDocument.Id);
        dto.Details.Title.ShouldBe(activityRuleDocument.Details.Title);
        dto.Details.Note.ShouldBe(activityRuleDocument.Details.Note);
        dto.Mode.Mode.ShouldBe(activityRuleDocument.SelectedMode.Mode);
        dto.Mode.Days?.ToList().IsEqual(activityRuleDocument.SelectedMode.DaysOfWeek?.ToList()).ShouldBeTrue();
        var stage = dto.Stages.Single();
        stage.StageId.ShouldBe(stageDocument.StageId);
        stage.Title.ShouldBe(stageDocument.Title);
        stage.Index.ShouldBe(stageDocument.Index);
    }
}