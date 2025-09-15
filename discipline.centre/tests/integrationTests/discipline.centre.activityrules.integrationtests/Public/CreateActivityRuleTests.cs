using System.Net;
using System.Net.Http.Json;
using discipline.centre.activity_rules.infrastructure.DAL.Documents;
using discipline.centre.activity_rules.tests.shared_kernel.Infrastructure;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.integrationtests.sharedkernel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationtests.Public;

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
    
    [Fact]
    public async Task GivenInvalidActivityRuleRequest_WhenCallTo_api_activity_rules_ShouldReturn422StatusCodeWithErrorCode()
    {
        // Arrange
        await AuthorizeWithFreeSubscriptionPicked();
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto(string.Empty, null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
         
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules", request);
         
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.Title.ShouldBe("ActivityRule.Validation.Details.Title.Empty");
    }
    
    [Fact]
    public async Task GivenUnauthorized_WhenCallTo_api_activity_rules_ShouldReturn401StatusCode()
    {
        // Arrange
        var command = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules", command);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
     
    [Fact]
    public async Task GivenWithoutActiveSubscription_WhenCallTo_api_activity_rules_ShouldReturn403StatusCode()
    {
        // Arrange
        await AuthorizedWithExpiredToken();
        var command = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/activity-rules", command);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}