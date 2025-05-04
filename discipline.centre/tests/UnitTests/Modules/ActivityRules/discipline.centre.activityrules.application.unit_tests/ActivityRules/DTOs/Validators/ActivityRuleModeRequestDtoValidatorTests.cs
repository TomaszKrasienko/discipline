using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.DTOs.Validators;

public sealed class ActivityRuleModeRequestDtoValidatorTests
{
    [Fact]
    public void GivenNotEmptyTitle_WhenTestValidate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var dto = new ActivityRuleModeRequestDto("test_mode", null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();   
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenTestValidate_ShouldHaveErrorForTitleWithCodeValidation_EmptyActivityRuleTitle()
    {
        // Arrange
        var dto = new ActivityRuleModeRequestDto(string.Empty, null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Mode);
        result.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleModeRequestDto.Mode) 
                && x.ErrorMessage == "Validation.EmptyActivityRuleMode")
            .ShouldBeTrue();
    }
    
    [Fact]
    public void GivenDayOutOfRange_WhenTestValidate_ShouldHaveErrorForDaysWithCodeValidation_ActivityRuleDaysOutOfRange()
    {
        // Arrange
        var dto = new ActivityRuleModeRequestDto("test_mode", [7]);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Days);
        result.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleModeRequestDto.Days) 
                && x.ErrorMessage == "Validation.ActivityRuleDaysOutOfRange")
            .ShouldBeTrue();
    }
    
    private readonly ActivityRuleModeRequestDtoValidator _validator = new();
}