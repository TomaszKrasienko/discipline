using System.Net;
using discipline.centre.activityrules.domain;
using discipline.centre.activityrules.domain.Enums;
using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.activityrules.infrastructure.DAL.Documents;
using discipline.centre.dailytrackers.application.ActivityRules.Clients.DTOs;
using discipline.centre.integrationTests.sharedKernel;
using discipline.centre.integrationTests.sharedKernel.Serialization;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using Shouldly;
using Xunit;

namespace discipline.centre.activityrules.integrationTests.Public;

[Collection("activity-rules-module-get-activity-rule-by-id")]
public sealed class GetByIdActivityRuleTests() : BaseTestsController("activity-rules-module")
{
    [Fact]
    public async Task GetById_GivenExistingId_ShouldReturn200OkStatusCodeAndActivityRuleDto()
    {
        //arrange
        var user = await AuthorizeWithFreeSubscriptionPicked();
        var activityRule = ActivityRule.Create(ActivityRuleId.New(), user.Id, 
            new ActivityRuleDetailsSpecification("test_title",null), 
            new ActivityRuleModeSpecification(RuleMode.EveryDay, null));
        
        await TestAppDb.GetCollection<ActivityRuleDocument>()
            .InsertOneAsync(activityRule.AsDocument());
        
        //act
        var response = await HttpClient.GetAsync($"api/activity-rules-module/activity-rules/{activityRule.Id.ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var textResult = await response.Content.ReadAsStringAsync();
        var result = SerializerForTests.Deserialize<ActivityRuleDto>(textResult);
        result!.ActivityRuleId.ShouldBe(activityRule.Id);
    }

    [Fact]
    public async Task GetById_GivenNotExistingId_ShouldReturn404NotFoundStatusCode()
    {
        //arrange
        await AuthorizeWithFreeSubscriptionPicked();
        
        //act
        var response = await HttpClient.GetAsync($"api/activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_Unauthorized_ShouldReturn401Unauthorized()
    {
        //act
        var response = await HttpClient.GetAsync($"api/activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task GetById_AuthorizedWithoutSubscription_ShouldReturn403ForbiddenStatusCode()
    {
        //arrange
        await AuthorizeWithoutSubscription();
        
        //act
        var response = await HttpClient.GetAsync($"api/activity-rules-module/activity-rules/{ActivityRuleId.New().ToString()}");
        
        //assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}