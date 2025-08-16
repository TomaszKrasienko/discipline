using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Activities;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.ValueObjects.Activities;

public sealed class DetailsTests
{
    [Fact]
    public void GivenValidArguments_WhenCreate_ThenReturnDetailsWithValues()
    {
        // Arrange
        var title = "test_title";
        var note = "test_note";
        
        // Act
        var result = Details.Create(title, note);
        
        // Assert
        result.Title.ShouldBe(title);
        result.Note.ShouldBe(note);
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowDomainExceptionWithCodeDailyTrackerActivityDetailsTitleEmpty()
    {
        // Act
        var exception = Record.Exception(() => Details.Create(string.Empty, null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Details.Title.Empty");
    }

    [Fact]
    public void GivenTitleLongerThan30Characters_WhenCreate_TheneThrowDomainExceptionWithCodeDailyTrackerActivityDetailsTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Details.Create(new string('t', 31), null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Activity.Details.Title.TooLong");
    }
}