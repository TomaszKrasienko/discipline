using System.Net;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.integrationtests.sharedkernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationtests.Public;

[Collection("activity-rules-module")]
public sealed class DeleteActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task GivenExistingActivityRule_WhenCallTo_api_activity_rules_activityRuleId_ThenReturns204StatusCode_Removes_SendsEvent()
    {
        // Arrange
        var account = await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), account.Id, 
            new ActivityRuleDetailsSpecification("test_title",null), 
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));
        
        await TestAppDb
            .GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule.ToDocument());
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/activity-rules/{activityRule.Id}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var doesActivityRuleExist = await TestAppDb.GetCollection<ActivityRuleDocument>()
            .Find(x 
                => x.Id == activityRule.Id.ToString() &&
                   x.AccountId == account.Id.ToString())
            .AnyAsync();
        doesActivityRuleExist.ShouldBeFalse();
    }
    
    [Fact]
    public async Task GivenNotExistingActivityRule_WhenCallTo_api_activity_rules_activityRuleId_ShouldReturn204StatusCode()
    {
        // Arrange
        _ = await AuthorizeWithFreeSubscriptionPicked();
        var activityRuleId = ActivityRuleId.New().ToString();
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/activity-rules/{activityRuleId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task GivenUnauthorized_WhenCallTo_api_activity_rules_activityRuleId_ShouldReturn401StatusCode()
    {
        // Arrange
        var activityRuleId = ActivityRuleId.New().ToString();
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/activity-rules/{activityRuleId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GivenTokenWithExpiredSubscription_WhenCallTo_api_activity_rules_activityRuleId_ShouldReturn403StatusCode()
    {
        // Arrange
        await AuthorizedWithExpiredToken();
        var activityRuleId = ActivityRuleId.New().ToString();
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/activity-rules/{activityRuleId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}