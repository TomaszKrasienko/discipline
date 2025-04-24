using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.tests.sharedkernel.DataValidators;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unit_tests.ActivityRulesTests;

public sealed class CreateTests
{
    [Fact]
    public void GivenValidArgumentsForModeWithoutDays_WhenCreate_ThenReturnActivityRuleWithValues()
    {
        // Arrange
        var activityRuleId = ActivityRuleId.New();
        var userId = UserId.New();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.EveryDay, null);
        
        // Act
        var result = ActivityRule.Create(activityRuleId, userId, details, mode);
        
        // Assert
        result.Id.ShouldBe(activityRuleId);
        result.UserId.ShouldBe(userId);
        result.Details.Title.ShouldBe(details.Title);
        result.Details.Note.ShouldBe(details.Note);
        result.Mode.Mode.ShouldBe(mode.Mode);
        result.Mode.Days.ShouldBeNull();
    }

    [Fact]
    public void GivenValidArgumentsForModeWithDays_WhenCreate_ThenReturnActivityRuleWithValues()
    {        
        // Arrange
        var activityRuleId = ActivityRuleId.New();
        var userId = UserId.New();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        
        // Act
        var result = ActivityRule.Create(activityRuleId, userId, details, mode);
        
        // Assert
        result.Id.ShouldBe(activityRuleId);
        result.UserId.ShouldBe(userId);
        result.Details.Title.ShouldBe(details.Title);
        result.Details.Note.ShouldBe(details.Note);
        result.Mode.Mode.ShouldBe(mode.Mode);
        result.Mode.Days.IsEqual(mode.Days?.ToList()).ShouldBeTrue();
    }

    [Fact]
    public void GivenEmptyTitle_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsEmptyTitle()
    {
        var (activityId, userId, details, mode) = GetFilledParams(); 
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(activityId, userId, 
            details with {Title = string.Empty}, mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.EmptyTitle");
    }

    [Fact]
    public void GivenTitleLongerThan30Characters_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleDetailsTitleTooLonge()
    {
        var (activityId, userId, details, mode) = GetFilledParams(); 
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(activityId, userId, 
            details with {Title = new string('t', 31)}, mode));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Details.TitleTooLong");
    }

    [Fact]
    public void GivenModeWithRequiredDaysAndNullDays_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRulesModeRuleModeRequireSelectedDays()
    {
        // Arrange
        var (activityId, userId, details, _) = GetFilledParams();
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(activityId, userId,
            details, new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: null)));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeRequireSelectedDays");
    }

    [Fact]
    public void GivenSelectedDaysOutOfRange_WhenCreate_ThenThrowDomainExceptionWithCodeActivityRuleModeSelectedDayOutOfRange()
    {
        // Arrange
        var  (activityId, userId, details, _) = GetFilledParams();
        
        // Act
        var exception = Record.Exception(() => ActivityRule.Create(activityId, userId, details,
            new ActivityRuleModeSpecification(Mode: RuleMode.Custom, Days: [7])));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeSelectedDayOutOfRange");
    }

    private static ActivityRuleCreateTestsParams GetFilledParams()
    {
        var activityRuleId = ActivityRuleId.New();
        var userId = UserId.New();
        var details = new ActivityRuleDetailsSpecification("test_title",
            "test_note");
        var mode = new ActivityRuleModeSpecification(RuleMode.Custom, [1,2,3]);
        return new ActivityRuleCreateTestsParams(activityRuleId, userId, details, mode);
    }
}

public record ActivityRuleCreateTestsParams(
    ActivityRuleId ActivityRuleId,
    UserId UserId,
    ActivityRuleDetailsSpecification Details,
    ActivityRuleModeSpecification Mode);
