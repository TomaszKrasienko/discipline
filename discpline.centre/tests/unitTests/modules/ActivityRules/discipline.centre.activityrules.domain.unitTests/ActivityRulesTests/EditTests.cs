using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.tests.sharedkernel.DataValidators;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unitTests.ActivityRulesTests;

public sealed class EditTests
{
    [Fact]
    public void GivenValidArgumentsForModeWithoutDays_WhenEdit_ThenReturnActivityRuleWithValues()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.EveryDay, null);
        
        // Act
        activityRule.Edit(details, mode);
        
        // Assert
        activityRule.Details.Title.ShouldBe(details.Title);
        activityRule.Details.Note.ShouldBe(details.Note);
        activityRule.Mode.Mode.ShouldBe(mode.Mode);
        activityRule.Mode.Days.ShouldBeNull();
    }

    [Fact]
    public void GivenValidArgumentsForModeWithDays_WhenEdit_ThenReturnActivityRuleWithValues()
    {        
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        
        // Act
        activityRule.Edit(details, mode);
        
        // Assert
        activityRule.Details.Title.ShouldBe(details.Title);
        activityRule.Details.Note.ShouldBe(details.Note);
        activityRule.Mode.Mode.ShouldBe(mode.Mode);
        activityRule.Mode.Days.IsEqual(mode.Days?.ToList()).ShouldBeTrue();
    }

    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsEmptyTitle()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParams(); 
        
        // Act
        var exception = Record.Exception(() => activityRule.Edit(details with {Title = string.Empty}, mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.EmptyTitle");
    }

    [Fact]
    public void GivenTitleLongerThan30Characters_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsTitleTooLonge()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParams();
        
        // Act
        var exception = Record.Exception(() => activityRule.Edit(details with {Title = new string('t', 31)}, mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.TitleTooLong");
    }

    [Fact]
    public void GivenModeWithRequiredDaysAndNullDays_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRulesModeRuleModeRequireSelectedDays()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParams();
        
        // Act
        var exception = Record.Exception(() => activityRule.Edit(details, 
            new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: null)));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeRequireSelectedDays");
    }

    [Fact]
    public void GivenSelectedDaysOutOfRange_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleModeSelectedDayOutOfRange()
    {
        // Arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var (details, mode) = GetFilledParams();
        
        // Act
        var exception = Record.Exception(() => activityRule.Edit(details, new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: [7])));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeSelectedDayOutOfRange");
    }

    private static ActivityRuleEditTestsParams GetFilledParams()
    {
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        return new ActivityRuleEditTestsParams(details, mode);
    }
}

public record ActivityRuleEditTestsParams(
    ActivityRuleDetailsSpecification Details,
    ActivityRuleModeSpecification Mode);