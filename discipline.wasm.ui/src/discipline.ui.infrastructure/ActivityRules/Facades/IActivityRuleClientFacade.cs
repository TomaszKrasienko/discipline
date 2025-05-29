using discipline.ui.communication.http.ActivityRules;
using Microsoft.Extensions.Logging;
using OneOf;

namespace discipline.ui.infrastructure.ActivityRules.Facades;

public interface IActivityRuleClientFacade
{
    Task<OneOf<bool, string>> DeleteActivityRuleStageAsync(
        string activityRuleId,
        string stagesId,
        CancellationToken cancellationToken);
}

internal sealed class ActivityRuleClientFacade(
    ILogger<ActivityRuleClientFacade> logger,
    IActivityRulesHttpService activityRuleClient) : BaseFacade<ActivityRuleClientFacade>(logger), IActivityRuleClientFacade
{
    public async Task<OneOf<bool, string>> DeleteActivityRuleStageAsync(string activityRuleId, string stagesId, CancellationToken cancellationToken)
    {
        var response = await activityRuleClient.DeleteActivityRuleStage(activityRuleId, stagesId, cancellationToken);
        return await ValidateResponse(response, cancellationToken);
    }
}