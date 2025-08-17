using System.Net;
using System.Net.Http.Json;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.domain.ValueObjects.ActivityRules;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Domain;
using discipline.centre.activityrules.tests.sharedkernel.Infrastructure;
using discipline.centre.integrationTests.sharedKernel;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationTests.Public;

[Collection("activity-rules-module")]
public sealed class CreateActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task GivenActivityRuleRequest_WhenCallTo_api_activity_rules_ThenReturn201StatusCode_Saves_SendsEvent()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
         
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules", request);
         
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
         
        var resourceId = GetResourceIdFromHeader(response);
        resourceId.ShouldNotBeNull();

        var activityRuleDocument = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id == resourceId)
            .SingleOrDefaultAsync();
        
        activityRuleDocument.ShouldNotBeNull();
        activityRuleDocument.AccountId.ShouldBe(account.Id.ToString());
        //TODO: Events to test
    }
    
    [Fact]
    public async Task GivenAlreadyRegisteredActivityRuleRequest_WhenCallTo_api_activity_rules_ThenReturn400StatusCode()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();

        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory.Get();
        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRuleDocument with {AccountId = account.Id.ToString()});
        
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto(activityRuleDocument.Details.Title, null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
         
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    // [Fact]
    // public async Task Create_GivenInvalidRequest_ShouldReturn422UnprocessableEntityStatusCode()
    // {
    //     //arrange
    //     await AuthorizeWithFreeSubscriptionPicked();
    //     var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
    //         new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
    //      
    //     //act
    //     var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
    //      
    //     //assert
    //     response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    // }
    //
    // [Fact]
    // public async Task Create_Unauthorized_ShouldReturn401UnauthorizedStatusCode()
    // {
    //     //arrange
    //     var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
    //         new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
    //     
    //     //act
    //     var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
    //     
    //     //assert
    //     response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    // }
    //  
    // [Fact]
    // public async Task Create_AuthorizedByUserWithStatusCreated_ShouldReturn403ForbiddenStatusCode()
    // {
    //     //arrange
    //     await AuthorizeWithoutSubscription();
    //     var command = new ActivityRuleRequestDto(new ActivityRuleDetailsRequestDto("test_title", null),
    //         new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
    //     
    //     //act
    //     var response = await HttpClient.PostAsJsonAsync("api/activity-rules-module/activity-rules", command);
    //     
    //     //assert
    //     response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    // }
}