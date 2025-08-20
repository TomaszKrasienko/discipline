using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.DTOs.Validators;

public sealed class ActivityRuleRequestDtoValidatorTests
{
    [Fact]
    public void GivenValidActivityRuleRequestDto_WhenTestValidate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto("test_description", [1]));
        
        // Act
        var validationResult = _validator.TestValidate(request);
        
        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GivenNullDetails_WhenTestValidate_ShouldHaveValidationErrorForDetailsWithErrorCode_ActivityRule_Validation_Details_Null()
    {
        // Arrange
        var request = new ActivityRuleRequestDto(
            null!,
            new ActivityRuleModeRequestDto("test_mode", [1]));
        
        // Act
        var validationResult = _validator.TestValidate(request);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Details);
        validationResult.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleRequestDto.Details) &&
                   x.ErrorCode == "ActivityRule.Validation.Details.Null")
            .ShouldBeTrue();
    }
    
    [Fact]
    public void GivenNullMode_WhenTestValidate_ShouldHaveValidationErrorForModeWithErrorCode_ActivityRule_Validation_Mode_Null()
    {
        // Arrange
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("test_title", null),
            null!);
        
        // Act
        var validationResult = _validator.TestValidate(request);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Mode);
        validationResult.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleRequestDto.Mode) &&
                   x.ErrorCode == "ActivityRule.Validation.Mode.Null")
            .ShouldBeTrue();
    }
    
    private readonly ActivityRuleRequestDtoValidator _validator = new();
}