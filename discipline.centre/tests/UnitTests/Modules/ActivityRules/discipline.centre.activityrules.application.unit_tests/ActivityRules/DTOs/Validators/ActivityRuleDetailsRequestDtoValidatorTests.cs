using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unit_tests.ActivityRules.DTOs.Validators;

public sealed class ActivityRuleDetailsRequestDtoValidatorTests
{
    [Fact]
    public void GivenNotEmptyTitle_WhenTestValidate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var dto = new ActivityRuleDetailsRequestDto("test_title", null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();   
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenTestValidate_ShouldHaveErrorForTitleWithCodeValidation_EmptyActivityRuleTitle()
    {
        // Arrange
        var dto = new ActivityRuleDetailsRequestDto(string.Empty, null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleDetailsRequestDto.Title)
                && x.ErrorMessage == "Validation.EmptyActivityRuleTitle")
            .ShouldBeTrue();
    }
    
    [Fact]
    public void GivenTitleLongerThan30Characters_WhenTestValidate_ShouldHaveErrorForTitleWithCodeValidation_ActivityRuleTitleTooLong()
    {
        // Arrange
        var dto = new ActivityRuleDetailsRequestDto(string.Empty, null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleDetailsRequestDto.Title)
                && x.ErrorMessage == "Validation.ActivityRuleTitleTooLong")
            .ShouldBeTrue();
    }
    
    private readonly ActivityRuleDetailsRequestDtoValidator _validator = new();
}