using discipline.centre.daily_trackers.tests.shared_kernel.Infrastructure;
using discipline.centre.dailytrackers.infrastructure.DAL.DailyTrackers.Documents;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.infrastructure.unitTests.DAL.DailyTrackers;

public sealed class ActivityDocumentMappingExtensionsTests
{
    [Fact]
    public void AsDto_WhenActivityDocumentWithoutStages_ShouldReturnActivityDtoWithoutStages()
    {
        //arrange
        var activityDocument = ActivityDocumentFakeDataFactory.Get(true, true, null);
        
        //act
        var result = activityDocument.AsDto();

        //assert
        result.ActivityId.ShouldBe(activityDocument.ActivityId);
        result.Details.Title.ShouldBe(activityDocument.Title);
        result.Details.Note.ShouldBe(activityDocument.Note);
        result.IsChecked.ShouldBe(activityDocument.IsChecked);
        result.ParentActivityRuleId.ShouldBe(activityDocument.ParentActivityRuleId!);
        result.Stages.ShouldBeNull();
    }
    
    [Fact]
    public void AsDto_WhenActivityDocumentWithStages_ShouldReturnActivityDtoWithStages()
    {
        //arrange
        var stageDocument = StageDocumentFakeDataFactory.Get(1);
        var activityDocument = ActivityDocumentFakeDataFactory.Get(true, true, [stageDocument]);
        
        //act
        var result = activityDocument.AsDto();

        //assert
        result.ActivityId.ShouldBe(activityDocument.ActivityId);
        result.Details.Title.ShouldBe(activityDocument.Title);
        result.Details.Note.ShouldBe(activityDocument.Note);
        result.IsChecked.ShouldBe(activityDocument.IsChecked);
        result.ParentActivityRuleId.ShouldBe(activityDocument.ParentActivityRuleId!);
        result.Stages!.Count.ShouldBe(1);
        result.Stages.First().Title.ShouldBe(stageDocument.Title);
        result.Stages.First().Index.ShouldBe(stageDocument.Index);
        result.Stages.First().IsChecked.ShouldBe(stageDocument.IsChecked);
    }
}