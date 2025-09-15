using discipline.centre.daily_trackers.tests.shared_kernel.Application;
using discipline.centre.dailytrackers.application.DailyTrackers.DTOs;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.dailytrackers.application.unitTests.DailyTrackers.Mappers;

public sealed class CreateActivityDtoMapperExtensionsTests
{    
    [Fact]
    public void MapAsCommand_GivenCreateActivityDtoWithoutStagesAndActivityIdAndUserId_ShouldMapToCommand()
    {
        //arrange
        var createActivityDto = CreateActivityDtoFakeDataFactory.Get(false);
        var activityId = ActivityId.New();
        var accountId = AccountId.New();
        
        //act
        var result = createActivityDto.MapAsCommand(accountId, activityId);
        
        //assert
        result.Day.ShouldBe(createActivityDto.Day);
        result.AccountId.ShouldBe(accountId);
        result.Details.Title.ShouldBe(createActivityDto.Details.Title);
        result.Details.Note.ShouldBe(createActivityDto.Details.Note);
        result.AccountId.ShouldBe(accountId);
        result.Stages.ShouldBeNull();
    }
    
    [Fact]
    public void MapAsCommand_GivenCreateActivityDtoWithStagesAndActivityIdAndUserId_ShouldMapToCommand()
    {
        //arrange
        var createActivityDto = CreateActivityDtoFakeDataFactory.Get(true);
        var activityId = ActivityId.New();
        var accountId = AccountId.New();
        
        //act
        var result = createActivityDto.MapAsCommand(accountId, activityId);
        
        //assert
        result.Day.ShouldBe(createActivityDto.Day);
        result.AccountId.ShouldBe(accountId);
        result.Details.Title.ShouldBe(createActivityDto.Details.Title);
        result.Details.Note.ShouldBe(createActivityDto.Details.Note);
        result.AccountId.ShouldBe(accountId);
        result.Stages![0].Title.ShouldBe(createActivityDto.Stages![0].Title);
        result.Stages![0].Index.ShouldBe(createActivityDto.Stages![0].Index);
    }
}