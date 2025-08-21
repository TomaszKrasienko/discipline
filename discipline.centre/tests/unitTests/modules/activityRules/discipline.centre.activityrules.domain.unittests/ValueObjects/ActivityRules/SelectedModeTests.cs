using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.tests.sharedkernel.DataValidators;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.domain.unittests.ValueObjects.ActivityRules;

public sealed class SelectedModeTests
{
    [Fact]
    public void GivenModeThatRequireSelectedDaysWithSelectedDays_WhenCreate_ThenReturnsSelectedModeWithValues()
    {
        // Arrange
        var mode = RuleMode.Custom;
        List<int> days = [1, 2, 3];
        
        // Act
        var result = SelectedMode.Create(mode, days.ToHashSet());
        
        // Assert
        result.Mode.ShouldBe(mode);
        result.Days.IsEqual(days).ShouldBeTrue();
    }
    
    [Fact]
    public void GivenModeThatNotRequireSelectedDaysWithoutSelectedDays_WhenCreate_ThenReturnsSelectedModeWithValues()
    {
        // Arrange
        var mode = RuleMode.EveryDay;
        
        // Act
        var result = SelectedMode.Create(mode, null);
        
        // Assert
        result.Mode.ShouldBe(mode);
        result.Days.ShouldBeNull();
    }

    [Fact]
    public void GivenModeThatRequireSelectedDaysWithoutSelectedDays_WhenCreate_ThenThrowsDomainExceptionWitCodeActivityRulesModeRuleModeRequireSelectedDays()
    {
        // Act
        var exception = Record.Exception(() => SelectedMode.Create(RuleMode.Custom, null));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeRequireSelectedDays");
    }
    
    [Fact]
    public void GivenDaysOutOfRange_WhenCreate_ThenThrowsDomainExceptionWithCodeActivityRuleModeSelectedDayOutOfRange()
    {
        // Act
        var exception = Record.Exception(() => SelectedMode.Create(RuleMode.Custom, [7]));
        
        // Assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRules.Mode.RuleModeSelectedDayOutOfRange");
    }
}