using discipline.ui.communication.http.ActivityRules;
using discipline.ui.communication.http.ActivityRules.DTOs.Requests;
using Microsoft.Extensions.Logging;
using OneOf;

namespace discipline.ui.infrastructure.ActivityRules.Facades;

public interface ICreateActivityRuleStageFacade
{
    Task<OneOf<bool, string>> HandleAsync(CreateActivityRuleStageRequestDto request, 
        string activityRuleId, 
        CancellationToken cancellationToken);
}

internal sealed class CreateActivityRuleStageFacade(
    ILogger<CreateActivityRuleStageFacade> logger,
    IActivityRulesHttpService activityRulesHttpService) : BaseFacade<CreateActivityRuleStageFacade>(logger), ICreateActivityRuleStageFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(CreateActivityRuleStageRequestDto request, string activityRuleId, CancellationToken cancellationToken)
    {
        var response = await activityRulesHttpService.CreateStage(activityRuleId, request, cancellationToken);
        return await ValidateResponse(response, cancellationToken);
    }
}