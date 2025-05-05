using System.Net;
using System.Net.Http.Json;
using discipline.ui.communication.http.ActivityRules;
using discipline.ui.communication.http.ActivityRules.DTOs.Requests;
using Microsoft.Extensions.Logging;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.ActivityRules.Facades;

public interface ICreateActivityRuleFacade
{
    Task<OneOf<bool, string>> HandleAsync(CreateActivityRuleRequestDto request, CancellationToken cancellationToken);
}

internal sealed class CreateActivityRuleFacade(
    ILogger<CreateActivityRuleFacade> logger,
    IActivityRulesHttpService activityRulesHttpService) : BaseFacade<CreateActivityRuleFacade>(logger), ICreateActivityRuleFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(CreateActivityRuleRequestDto request, CancellationToken cancellationToken)
    {
        var response = await activityRulesHttpService.CreateActivityRule(request, cancellationToken);
        return await ValidateResponse(response, cancellationToken);
    }
}