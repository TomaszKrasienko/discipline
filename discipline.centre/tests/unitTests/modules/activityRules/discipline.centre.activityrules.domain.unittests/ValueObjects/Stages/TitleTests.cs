using discipline.centre.activityrules.domain.ValueObjects.Stages;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unittests.ValueObjects.Stages;

public sealed class TitleTests
{
    [Fact]
    public void GivenValidValue_WhenCreate_ThenReturnsTitleWithValue()
    {
        // Arrange
        const string value = "test_title";
        
        // Act
        var result = Title.Create(value);
        
        // Assert
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void GivenEmptyValue_WhenCreate_ThenThrowsDomainExceptionActivityRuleStageTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Title.Create(string.Empty));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.Title.EmptyValue");
    }

    [Fact]
    public void GivenValueLongerThan30Characters_WhenCreate_ThenThrowsDomainExceptionWithCodeActivityRuleTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Title.Create(new string('t', 31)));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.Title.ValueTooLong");
    }
}