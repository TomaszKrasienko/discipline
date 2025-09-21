using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.daily_trackers.domain.tests;

public sealed class StageTests
{
    #region Arrange
    
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
    
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerStageTitleEmpty()
    {
        // Act
        var exception = Record.Exception(() => Stage.Create(StageId.New(), string.Empty, 1));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Title.Empty");
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerStageTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Stage.Create(StageId.New(), new string('t', 31), 1));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Title.TooLong");
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerStageIndexLessThanOne()
    {
        // Act
        var exception = Record.Exception(() => Stage.Create(StageId.New(), "test_title", 0));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Index.LessThanOne");
    }
    
    #endregion
    #region MarkAsChecked
    
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
    
    #endregion
}