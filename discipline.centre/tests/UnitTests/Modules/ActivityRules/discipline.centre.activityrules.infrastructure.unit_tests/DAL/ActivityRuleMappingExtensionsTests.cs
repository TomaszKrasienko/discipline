using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.infrastructure.unit_tests.DAL;

public sealed class ActivityRuleMappingExtensionsTests
{
    [Fact]
    public void GivenActivityRuleWithoutSelectedDays_WhenAsDocument_ThenReturnActivityRuleDocumentWithNullSelectedDays()
    {
        //arrange
        var activityRule = ActivityRuleFakeDataFactory.Get();
        
        //act
        var result = activityRule.AsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        result.SelectedMode.DaysOfWeek.ShouldBeNull();
    }
    
    [Fact]
    public void GivenActivityRuleWithSelectedDays_WhenAsDocument_ShouldReturnActivityRuleDocument()
    {
        //arrange
        List<int> selectedDays = [0, 1, 2];
        var activityRule = ActivityRuleFakeDataFactory.Get(true, selectedDays.ToHashSet());
        
        //act
        var result = activityRule.AsDocument();
        
        //assert
        result.Id.ShouldBe(activityRule.Id.ToString());
        result.UserId.ShouldBe(activityRule.UserId.ToString());
        result.Details.Title.ShouldBe(activityRule.Details.Title);
        result.Details.Note.ShouldBe(activityRule.Details.Note);
        result.SelectedMode.Mode.ShouldBe(activityRule.Mode.Mode.Value);
        result.SelectedMode.DaysOfWeek.ShouldBeEquivalentTo(selectedDays);
    }

    [Fact]
    public void GivenActivityRuleWithStae_WhenAsDocument_ThenActivityRuleShouldHaveStage()
    {
        
    }
}