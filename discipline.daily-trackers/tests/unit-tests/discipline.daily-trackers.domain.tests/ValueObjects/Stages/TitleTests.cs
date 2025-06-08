using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Stages;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.ValueObjects.Stages;

public sealed class TitleTests
{
    [Fact]
    public void GivenValidArguments_WhenCreate_ThenReturnTitleWithValue()
    {
        // Arrange
        const string value = "test_title";
        
        // Act
        var result = Title.Create(value);
        
        // Assert
        result.Value.ShouldBe(value);
    }
    
    [Fact]
    public void GivenEmptyValue_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleStageTitleEmpty()
    {
        // Act
        var exception = Record.Exception(() => Title.Create(string.Empty));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Title.Empty");
    }

    [Fact]
    public void GivenValueLongerThan30Characters_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleStageTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Title.Create(new string('a', 31)));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Title.TooLong");
    }
}