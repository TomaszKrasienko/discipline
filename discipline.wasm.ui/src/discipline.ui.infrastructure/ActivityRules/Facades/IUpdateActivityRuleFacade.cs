using System.Net;
using System.Net.Http.Json;
using discipline.ui.communication.http.ActivityRules;
using discipline.ui.communication.http.ActivityRules.DTOs.Requests;
using Microsoft.Extensions.Logging;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.ActivityRules.Facades;

public interface IUpdateActivityRuleFacade
{
    Task<OneOf<bool, string>> HandleAsync(UpdateActivityRuleRequestDto request,
        string activityRuleId,
        CancellationToken cancellationToken);
}

internal sealed class UpdateActivityRuleFacade(
    ILogger<UpdateActivityRuleFacade> logger,
    IActivityRulesHttpService activityRulesHttpService) : BaseFacade<UpdateActivityRuleFacade>(logger), IUpdateActivityRuleFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(UpdateActivityRuleRequestDto request, string activityRuleId, CancellationToken cancellationToken)
    {
        var response = await activityRulesHttpService.UpdateActivityRule(activityRuleId, request, cancellationToken);
        return await ValidateResponse(response, cancellationToken);
    }
}