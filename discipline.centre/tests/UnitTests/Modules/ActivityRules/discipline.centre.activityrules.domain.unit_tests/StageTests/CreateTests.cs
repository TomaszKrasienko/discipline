using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.StageTests;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidArguments_WhenCreate_ThenReturnStageWithValues()
    {
        // Arrange
        var id = StageId.New();
        const string title = "test_stage_value";
        const int index = 1;
        
        // Act
        var result = Stage.Create(id, title, index);
        
        // Assert
        result.Id.ShouldBe(id);
        result.Title.Value.ShouldBe(title);
        result.Index.Value.ShouldBe(index);
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowDomainExceptionActivityRuleStageTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Stage.Create(StageId.New(), string.Empty, 1));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.TitleTooLong");
    }
    
    [Fact]
    public void GivenTitleLongerThan30Characters_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleTitleTooLong()
    {
        // Act
        var exception = Record.Exception(() => Stage.Create(StageId.New(), string.Empty, 1));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.TitleTooLong");
    }
    
    [Fact]
    public void GivenNegativeOrderIndex_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleStageIndexLessThanOne()
    {
        // Act
        var exception = Record.Exception(() => Stage.Create(StageId.New(), "test_stage_value", -1));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stage.Index.ValueBelowOne");
    }
}