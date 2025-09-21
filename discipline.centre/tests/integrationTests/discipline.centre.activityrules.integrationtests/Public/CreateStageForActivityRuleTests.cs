using System.Net;
using System.Net.Http.Json;
using discipline.centre.activity_rules.infrastructure.DAL.Documents;
using discipline.centre.activity_rules.tests.shared_kernel.Infrastructure;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.Stages;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.integrationtests.sharedkernel;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationtests.Public;

[Collection("activity-rules-module")]
public class CreateStageForActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task GivenValidCreateStageRequest_WhenCallTo_api_activity_rules_activityRuleId_stages_ThenReturns204StatusCode_Saves()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();

        var activityRule = ActivityRuleDocumentFakeDataFactory
            .Get();
        
        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule with { AccountId = account.Id.ToString() });

        var request = new CreateStageRequestDto("new_stage_test", null);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/activity-rules/{activityRule.Id}/stages", request); 
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        (await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x 
                => x.Id == activityRule.Id &&
                   x.Stages.Any(s => s.Title == request.Title))
            .AnyAsync()).ShouldBeTrue();
    }
    
    [Fact]
    public async Task GivenCreateStageRequestWithNotUniqueTitle_WhenCallTo_api_activity_rules_activityRuleId_stages_ThenReturns400StatusCode()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();

        var activityRule = ActivityRuleDocumentFakeDataFactory
            .Get()
            .WithStage();
        
        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule with { AccountId = account.Id.ToString() });

        var request = new CreateStageRequestDto(activityRule.Stages.Single().Title, null);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/activity-rules/{activityRule.Id}/stages", request); 
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GivenNotExistingActivityRule_WhenCallTo_api_activity_rules_activityRuleId_stages_ThenReturns404StatusCode()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();
        var request = new CreateStageRequestDto("new_stage_test", null);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/activity-rules/{Ulid.NewUlid().ToString()}/stages", request); 
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GivenInvalidCreateStageRequest_WhenCallTo_api_activity_rules_activityRuleId_stages_ThenReturns422StatusCodeWithErrorCode()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();

        var activityRule = ActivityRuleDocumentFakeDataFactory
            .Get();
        
        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule with { AccountId = account.Id.ToString() });

        var request = new CreateStageRequestDto(string.Empty, null);

        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/activity-rules/{activityRule.Id}/stages", request); 
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result!.Title.ShouldBe("CreateStage.Validation.Title.Empty");
    }

    [Fact]
    public async Task GivenUnauthorized_WhenCallTo_api_activity_rules_activityRuleId_stages_ShouldReturn401StatusCode()
    {
        // Arrange
        var request = new CreateStageRequestDto("test_title", null);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/activity-rules/{Ulid.NewUlid().ToString()}/stages", request); 
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GivenWithoutActiveSubscription_WhenCallTo_api_activity_rules_activityRuleId_stages_ShouldReturn403StatusCode()
    {
        // Arrange
        await AuthorizedWithExpiredToken();
        var request = new CreateStageRequestDto("test_title", null);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync($"/api/activity-rules/{Ulid.NewUlid().ToString()}/stages", request); 
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}