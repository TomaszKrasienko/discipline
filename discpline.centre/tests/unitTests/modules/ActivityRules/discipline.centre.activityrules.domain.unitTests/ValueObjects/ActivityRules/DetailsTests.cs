using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unitTests.ValueObjects.ActivityRules;

public sealed class DetailsTests
{
    [Fact]
    public void GivenValidParameters_WhenCreate_ThenReturnDetailsWithValues()
    {
        //arrange
        var title = "test_title";
        var note = "test_note";
        
        //act
        var result = Details.Create(title, note);
        
        //assert
        result.Title.ShouldBe(title);
        result.Note.ShouldBe(note);
    }

    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsTitleEmpty()
    {
        //act
        var exception = Record.Exception(() => Details.Create(string.Empty, null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.EmptyTitle");
    }
    
    [Fact]
    public void GivenTitleLongerThan30_WhenCreate_WhenThrowDomainExceptionWithCodeActivityRuleDetailsTitleTooLong()
    {
        //act
        var exception = Record.Exception(() => Details.Create(new string('t', 31), null));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.TitleTooLong");
    }
}