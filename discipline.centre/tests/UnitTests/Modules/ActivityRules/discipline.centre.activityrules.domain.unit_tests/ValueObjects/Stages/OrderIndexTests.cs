using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ValueObjects.Stages;

public sealed class OrderIndexTests
{
    [Fact]
    public void GivenPositiveValue_WhenCreate_ThenReturnOrderIndexWithValue()
    {
        // Arrange
        const int value = 1;
        
        // Act
        var result = OrderIndex.Create(value);
        
        // Assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void GivenValueNegativeOrZero_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleStageIndexLessThanOne()
    {
        // Act
        var exception = Record.Exception(() => OrderIndex.Create(0));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.Index.ValueBelowOne");
    }
}