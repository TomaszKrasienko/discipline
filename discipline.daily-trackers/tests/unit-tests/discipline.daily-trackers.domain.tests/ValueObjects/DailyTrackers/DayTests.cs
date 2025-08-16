using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.ValueObjects.DailyTrackers;

public sealed class DayTests
{
    [Fact]
    public void GivenValidArguments_WhenCreate_ThenReturnDayWithValue()
    {
        // Arrange
        var value = DateOnly.FromDateTime(DateTime.Now);
        
        // Act
        var result = Day.Create(value);
        
        // Assert
        result.Value.ShouldBe(value);
    }
    
    [Fact]
    public void GivenDefaultDateOnly_WhenCreate_ThenThrowDomainExceptionWithCode()
    {
        // Act
        var exception = Record.Exception(() => Day.Create(default));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Day.Default");
    }
}