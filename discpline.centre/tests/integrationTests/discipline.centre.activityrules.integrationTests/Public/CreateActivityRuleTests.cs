using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.integrationTests.sharedKernel;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationTests.Public;

[Collection("activity-rules-module-create-activity-rule")]
public sealed class CreateActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task Create_GivenValidParameters_ShouldReturn201CreatedStatusCodeAndAddToDb()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
         
        //act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
         
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
         
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();

        var newActivityRuleDocument = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id.ToString() == resourceId)
            .SingleOrDefaultAsync(); 

        newActivityRuleDocument.ShouldNotBeNull();
        newActivityRuleDocument.UserId.ShouldBe(user.Id.ToString());
    }
    
    [Fact]
    public async Task Create_GivenAlreadyExistingTitle_ShouldReturn400BadRequestStatusCode()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRuleFakeDataFactory.Get();
        var activityRuleDocument = activityRule.AsDocument();
            
        await TestAppDb.GetCollection<ActivityRuleDocument>().InsertOneAsync(activityRuleDocument with { UserId = user.Id.ToString() });
        var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
         
        //act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
         
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_GivenInvalidRequest_ShouldReturn422UnprocessableEntityStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
         
        //act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
         
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task Create_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    {
        //arrange
        var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
        
        //act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
     
    [Fact]
    public async Task Create_AuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
        
        //act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}