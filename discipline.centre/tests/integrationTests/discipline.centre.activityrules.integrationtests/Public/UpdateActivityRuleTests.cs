using System.Net;
using System.Net.Http.Json;
using discipline.centre.activity_rules.infrastructure.DAL.Documents;
using discipline.centre.activity_rules.tests.shared_kernel.Infrastructure;
using discipline.centre.activityrules.application.ActivityRules.DTOs.Requests.ActivityRules;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.integrationtests.sharedkernel;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationtests.Public;

[Collection("activity-rules-module")]
public sealed class UpdateActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task GivenExistingActivityRuleWithValidArguments_WhenCallTo_api_activity_rules_activityRuleId_ThenReturns204StatusCode_Updates_SendsEvent()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory
            .Get();

        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRuleDocument with {AccountId = account.Id.ToString()});

        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("new_test_title", "new_test_note"),
            new ActivityRuleModeRequestDto(RuleMode.Custom.Value, [1]));

        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/activity-rules/{activityRuleDocument.Id}", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
        var updatedActivityRuleDocument = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id.ToString() == activityRuleDocument.Id)
            .SingleOrDefaultAsync(); 
        updatedActivityRuleDocument.Details.Title.ShouldBe(request.Details.Title);
        updatedActivityRuleDocument.Details.Note.ShouldBe(request.Details.Note);
        updatedActivityRuleDocument.SelectedMode.Mode.ShouldBe(request.Mode.Mode);
        updatedActivityRuleDocument.SelectedMode.DaysOfWeek!.First().ShouldBe(request.Mode.Days![0]);
    }
    
    [Fact]
    public async Task GivenAlreadyExistingTitle_WhenCallTo_api_activity_rules_activityRuleId_ThenReturns400StatusCode()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();
        var activityRuleDocument1 = ActivityRuleDocumentFakeDataFactory.Get();
        var activityRuleDocument2 = ActivityRuleDocumentFakeDataFactory.Get();

        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertManyAsync([
                activityRuleDocument1 with { AccountId = account.Id.ToString() },
                activityRuleDocument2 with { AccountId = account.Id.ToString() }
            ]);
        
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto(activityRuleDocument1.Details.Title, null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
    
        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/activity-rules/{activityRuleDocument2.Id}", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GivenInvalidUpdateActivityRuleRequestDto_WhenCallTo_api_activity_rules_activityRuleId_ThenReturns422StatusCode()
    {
        // Arrange
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto(string.Empty, "new_test_note"),
            new ActivityRuleModeRequestDto(RuleMode.Custom.Value, [0]));
        await AuthorizeWithFreeSubscriptionPicked();
        
        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/activity-rules/{Ulid.NewUlid().ToString()}", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }
    
    [Fact]
    public async Task GivenUnauthorized_WhenCallTo_api_activity_rules_activityRuleId_ThenReturns401StatusCode()
    {
        // Arrange
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
    
        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/activity-rules/{Ulid.NewUlid().ToString()}", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GivenTokenWithExpiredSubscription_WhenCallTo_api_activiy_rules_activityRuleId_ThenReturns403StatusCode()
    {
        // Arrange
        var request = new ActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto("test_title", null),
            new ActivityRuleModeRequestDto(RuleMode.EveryDay.Value, null));
        await AuthorizedWithExpiredToken();
        
        // Act
        var response = await HttpClient.PutAsJsonAsync($"api/activity-rules/{Ulid.NewUlid().ToString()}", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}