using System.Net;
using System.Net.Http.Json;
using discipline.hangfire.activity_rules.Clients;
using discipline.hangfire.activity_rules.DTOs;
using discipline.hangfire.activity_rules.Events.External.Handlers;
using discipline.hangfire.shared.abstractions.Identifiers;
using Microsoft.Extensions.Logging;
using OneOf;

namespace discipline.hangfire.activity_rules.Facades;

internal interface ICentreFacade
{
    Task<OneOf<bool, ActivityRuleDto>> GetActivityRule(UserId userId, ActivityRuleId activityRuleId, CancellationToken cancellationToken);
}

internal sealed class CentreFacade(ILogger<ActivityRuleRegisteredHandler> logger,
    ICentreActivityRuleClient client) : ICentreFacade
{
    public async Task<OneOf<bool, ActivityRuleDto>> GetActivityRule(UserId userId, ActivityRuleId activityRuleId, CancellationToken cancellationToken)
    {
        var activityRuleResponse = await client.GetActivityRule(userId.ToString(), activityRuleId.ToString(), 
            cancellationToken);

        if (activityRuleResponse.StatusCode is HttpStatusCode.InternalServerError)
        {
            var error = await activityRuleResponse.Content.ReadAsStringAsync(cancellationToken);
            logger.LogError($"Internal server error: {error}");
            return false;
        }
        
        if (!activityRuleResponse.IsSuccessStatusCode)
        {
            logger.LogError("Activity Rule with 'Id': {0} and 'UserId': {1} not found", 
                activityRuleId.ToString(), userId.ToString());
            return false;
        }

        var test = await activityRuleResponse.Content.ReadAsStringAsync(cancellationToken);
        
        var activityRuleResult = await activityRuleResponse.Content.ReadFromJsonAsync<ActivityRuleDto>(cancellationToken);

        if (activityRuleResult is null)
        {
            logger.LogError("Activity Rule with 'Id': {0} and 'UserId': {1} not found", 
                activityRuleId.ToString(), userId.ToString());
            return false;
        }
        
        return activityRuleResult;
    }
}