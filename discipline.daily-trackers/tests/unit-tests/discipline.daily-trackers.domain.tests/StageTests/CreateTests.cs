using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.StageTests;

public partial class CreateTests
{
    [Fact]
    public void GivenValidArguments_WhenCreate_ThenReturnStageWithExpectedValuesAndIsCheckedAsFalse()
    {
        // Arrange
        var stageId = StageId.New();
        const string title = "test_stage_title";
        const int index = 1;
        
        // Act
        var result = Stage.Create(stageId, title, index);
        
        // Assert
        result.Id.ShouldBe(stageId);
        result.Title.Value.ShouldBe(title);
        result.Index.Value.ShouldBe(index);
        result.IsChecked.Value.ShouldBeFalse();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateData))]
    public void GivenInvalidArgument_WhenCreate_ThenThrowDomainExceptionWithCode(CreateTestParameters parameters, string code)
    {
        // Act
        var exception = Record.Exception(() => Stage.Create(parameters.StageId, parameters.Title, parameters.Index));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }
}