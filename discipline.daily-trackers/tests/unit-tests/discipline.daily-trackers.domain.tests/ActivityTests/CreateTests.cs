using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using Shouldly;

namespace discipline.daily_trackers.domain.tests.ActivityTests;

public partial class CreateTests
{
    [Fact]
    public void GivenValidArgumentsWithStage_WhenCreate_ShouldReturnActivityWithStage()
    {
        // Arrange
        const string stageTitle = "test_stage_title";
        const int index = 1;
        
        // Act
        var result = Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title", null),
            null, [new StageSpecification(stageTitle, index)]);
        
        // Assert
        result.Stages.First().Title.Value.ShouldBe(stageTitle);
        result.Stages.First().Index.Value.ShouldBe(index);
    }
    
    [Theory]
    [MemberData(nameof(GetValidCreateData))]
    public void GivenValidArguments_WhenCreate_ThenReturnActivityWithValues(CreateTestParameters parameters)
    {
        // Act
        var result = Activity.Create(parameters.ActivityId, parameters.Details, parameters.ActivityRuleId,
            parameters.Stages);
        
        // Assert
        result.Id.ShouldBe(parameters.ActivityId);
        result.Details.Title.ShouldBe(parameters.Details.Title);
        result.Details.Note.ShouldBe(parameters.Details.Note);
        result.ParentActivityRuleId.ShouldBe(parameters.ActivityRuleId);
        result.IsChecked.Value.ShouldBeFalse();
        result.Stages.ShouldBeNull();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCreateData))]
    public void GivenInvalidData_ShouldThrowDomainExceptionWithCode(CreateTestParameters parameters, string code)
    {
        //act
        var exception = Record.Exception(() => Activity.Create(parameters.ActivityId, parameters.Details, parameters.ActivityRuleId,
            parameters.Stages));
        
        //assert
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe(code);
    }

    [Fact]
    public void GivenInvalidStageIndex_ShouldThrowDomainExceptionWithCode()
    {
        //act
        var exception = Record.Exception(() => Activity.Create(ActivityId.New(), new ActivityDetailsSpecification("test_title", null),
             null, [new StageSpecification("test_stage_title", 2)]));
        
        //assert 
        exception.ShouldBeOfType<DomainException>();
        ((DomainException)exception).Code.ShouldBe("ActivityRule.Stages.MustHaveOrderedIndex");
    }
}