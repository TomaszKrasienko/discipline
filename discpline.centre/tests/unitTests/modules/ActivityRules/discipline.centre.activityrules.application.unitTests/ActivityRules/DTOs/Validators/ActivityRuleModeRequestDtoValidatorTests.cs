using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.DTOs.Validators;

public sealed class ActivityRuleModeRequestDtoValidatorTests
{
    [Fact]
    public void GivenValidActivityRuleModeRequestDto_WhenTestValidate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var dto = new ActivityRuleModeRequestDto("test_mode", [1,2]);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();   
    }
    
    [Fact]
    public void GivenEmptyMode_WhenTestValidate_ShouldHaveErrorForTitleWithErrorCode_Validation_Mode_Mode_Empty()
    {
        // Arrange
        var dto = new ActivityRuleModeRequestDto(string.Empty, null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Mode);
        result.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleModeRequestDto.Mode) &&
                   x.ErrorCode == "ActivityRule.Validation.Mode.Mode.Empty")
            .ShouldBeTrue();
    }
    
    [Fact]
    public void GivenDayOutOfRange_WhenTestValidate_ShouldHaveErrorForDaysWithErrorCode_Validation_Mode_Days_OutOfRange()
    {
        // Arrange
        var dto = new ActivityRuleModeRequestDto("test_mode", [7]);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Days);
        result.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleModeRequestDto.Days) &&
                   x.ErrorCode == "ActivityRule.Validation.Mode.Days.OutOfRange")
            .ShouldBeTrue();
    }
    
    private readonly ActivityRuleModeRequestDtoValidator _validator = new();
}