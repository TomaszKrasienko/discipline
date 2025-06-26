using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.StageTests;

public sealed class MarkAsCheckedTests
{
    [Fact]
    public void GivenStage_WhenMarkAsChecked_ThenSetsIsCheckedToTrue()
    {
        // Arrange
        var stage = Stage.Create(StageId.New(), "test_stage_title", 1);
        
        // Act
        stage.MarkAsChecked();
        
        // Assert
        stage.IsChecked.Value.ShouldBeTrue();
    }
}