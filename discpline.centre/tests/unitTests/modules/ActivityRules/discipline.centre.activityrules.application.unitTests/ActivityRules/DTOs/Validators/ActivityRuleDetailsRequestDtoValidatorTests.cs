using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.DTOs.Validators;

public sealed class ActivityRuleDetailsRequestDtoValidatorTests
{
    [Fact]
    public void GivenValidActivityRuleDetailsRequestDto_WhenTestValidate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var dto = new ActivityRuleDetailsRequestDto("test_title", null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();   
    }
    
    [Fact]
    public void GivenEmptyTitle_WhenTestValidate_ShouldHaveErrorForTitleWithErrorCode_ActivityRule_Validation_Details_Title_Empty()
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
                && x.ErrorCode == "ActivityRule.Validation.Details.Title.Empty")
            .ShouldBeTrue();
    }
    
    [Fact]
    public void GivenTitleLongerThan30Characters_WhenTestValidate_ShouldHaveErrorForTitleWithCode_ActivityRule_Validation_Details_Title_TooLong()
    {
        // Arrange
        var dto = new ActivityRuleDetailsRequestDto(new string('t', 31), null);
        
        // Act
        var result = _validator.TestValidate(dto);
        
        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.Errors
            .Any(x 
                => x.PropertyName == nameof(ActivityRuleDetailsRequestDto.Title)
                && x.ErrorMessage == "ActivityRule.Validation.Details.Title.TooLong")
            .ShouldBeTrue();
    }
    
    private readonly ActivityRuleDetailsRequestDtoValidator _validator = new();
}