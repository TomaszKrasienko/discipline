using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.StageTests;

public sealed class UpdateIndexTests
{
    [Fact]
    public void GivenPositiveIndex_WhenUpdateIndex_ShouldChangeStageIndex()
    {
        // Arrange
        var stage = Stage.Create(StageId.New(), "test_title", 1);
        const int expectedIndex = 2;
        
        // Act
        stage.UpdateIndex(expectedIndex);
        
        // Assert
        stage.Index.Value.ShouldBe(expectedIndex);
    }
}