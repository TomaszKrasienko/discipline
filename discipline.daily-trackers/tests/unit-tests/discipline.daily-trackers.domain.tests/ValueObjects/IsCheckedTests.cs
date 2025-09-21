using discipline.daily_trackers.domain.DailyTrackers.ValueObjects;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.ValueObjects;

public sealed class IsCheckedTests
{
    [Fact]
    public void GivenValidArguments_WhenCreate_ShouldReturnIsChecked()
    {
        // Arrange
        const bool expected = true;
        
        // Act
        var result = IsChecked.Create(expected);
        
        // Assert
        result.Value.ShouldBe(expected);
    }
}