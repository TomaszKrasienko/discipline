using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unittests.ValueObjects.ActivityRules;

public sealed class DetailsTests
{
    [Fact]
    public void GivenValidParameters_WhenCreate_ThenReturnDetailsWithValues()
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
    public void GivenEmptyTitle_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsTitleEmpty()
    {
        // Act
        var exception = Record.Exception(() => Details.Create(string.Empty, null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.EmptyTitle");
    }
    
    [Fact]
    public void GivenTitleLongerThan30_WhenCreate_ThenThrowsDomainExceptionWithCodeActivityRuleDetailsTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Details.Create(new string('t', 31), null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.TitleTooLong");
    }
}