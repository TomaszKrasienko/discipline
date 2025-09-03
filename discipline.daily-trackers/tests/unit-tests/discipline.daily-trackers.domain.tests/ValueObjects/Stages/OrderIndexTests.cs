using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Stages;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.ValueObjects.Stages;

public sealed class OrderIndexTests
{
    [Fact]
    public void GivenValidArguments_WhenCreate_ThenReturnsOrderIndexWithValue()
    {
        // Arrange
        const int value = 1;
        
        // Act
        var result = OrderIndex.Create(value);
        
        // Assert
        result.Value.ShouldBe(value);
    }
    
    [Fact]
    public void GivenIndexLessThan0_WhenCreate_ThenThrowsDomainExceptionWithCodeDailyTrackerActivityStageIndexLessThanOne()
    {
        // Act
        var exception = Record.Exception(() => OrderIndex.Create(0));
        
        // Aesser
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("DailyTracker.Stage.Index.LessThanOne");
    }
}