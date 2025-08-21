using System.Net;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.activityrules.tests.sharedkernel.Infrastructure;
using discipline.centre.integrationtests.sharedkernel;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationtests.Public;

[Collection("activity-rules-module")]
public sealed class DeleteActivityRuleStageTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task GivenExistingActivityRuleStage_WhenCallTo_api_activity_rules_activityRuleId_stages_stageId_ThenReturns204StatusCode_Removes()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();
        var activityRuleDocument = ActivityRuleDocumentFakeDataFactory
            .Get()
            .WithStage();
        var stage = activityRuleDocument.Stages.Single();
        
        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRuleDocument with {AccountId = account.Id.ToString() });
        
        // Act 
        var response = await HttpClient.DeleteAsync($"/api/activity-rules/{activityRuleDocument.Id}/stages/{stage.StageId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var activityRule = await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .Find(x => x.Id == activityRuleDocument.Id)
            .SingleAsync();
        
        activityRule.Stages.Count().ShouldBe(0);
    }
    
    [Fact]
    public async Task GivenUnauthorized_WhenCallTo_api_activity_rules_activityRuleId_stages_stageId_ThenReturns401StatusCode()
    {
        // Act 
        var response = await HttpClient.DeleteAsync($"/api/activity-rules/{Ulid.NewUlid().ToString()}/stages/{Ulid.NewUlid().ToString()}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GivenTokenWithExpiredSubscription_WhenCallTo_api_activity_rules_activityRuleId_stages_stageId_ThenReturns403StatusCode()
    {
        // Arrange
        _ = await AuthorizedWithExpiredToken();
        
        // Act 
        var response = await HttpClient.DeleteAsync($"/api/activity-rules/{Ulid.NewUlid().ToString()}/stages/{Ulid.NewUlid().ToString()}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}