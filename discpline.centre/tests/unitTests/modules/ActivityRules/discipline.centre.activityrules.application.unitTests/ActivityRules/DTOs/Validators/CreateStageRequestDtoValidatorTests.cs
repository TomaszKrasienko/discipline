using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages.Validators;
using FluentValidation.TestHelper;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.application.unitTests.ActivityRules.DTOs.Validators;

public sealed class CreateStageRequestDtoValidatorTests
{
    [Fact]
    public void GivenValidCreateStageRequestDto_WhenTestValidate_ShouldNotHaveAnyValidationErrors()
    {
        // Arrange
        var requestDto = new CreateStageRequestDto("test_title", null);
        
        // Act
        var validationResult = _validator.TestValidate(requestDto);
        
        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void GivenEmptyTitle_WhenTestValidate_ShouldHaveErrorForTitleWithErrorCode_CreateStage_Validation_Title_Empty()
    {
        // Arrange
        var requestDto = new CreateStageRequestDto(string.Empty, null);
        
        // Act
        var validationResult = _validator.TestValidate(requestDto);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Title);
        validationResult.Errors
            .Any(x 
                => x.PropertyName == nameof(CreateStageRequestDto.Title)
                   && x.ErrorCode == "CreateStage.Validation.Title.Empty")
            .ShouldBeTrue();
    }
    
    [Fact]
    public void GivenTitleLongerThan30Characters_WhenTestValidate_ShouldHaveErrorForTitleWithErrorCode_CreateStage_Validation_Title_TooLong()
    {
        // Arrange
        var requestDto = new CreateStageRequestDto(new string('t', 31), null);
        
        // Act
        var validationResult = _validator.TestValidate(requestDto);
        
        // Assert
        validationResult.ShouldHaveValidationErrorFor(x => x.Title);
        validationResult.Errors
            .Any(x 
                => x.PropertyName == nameof(CreateStageRequestDto.Title)
                   && x.ErrorCode == "CreateStage.Validation.Title.TooLong")
            .ShouldBeTrue();
    }
    
    private readonly CreateStageRequestDtoValidator _validator = new ();
}