using discipline.ui.communication.http.DailyTrackers;
using Microsoft.Extensions.Logging;
using OneOf;

namespace discipline.ui.infrastructure.DailyTrackers.DailyTrackers;

public interface IDailyTrackerClientFacade
{
    Task<OneOf<bool, string>> CreateActivityFromActivityRuleAsync(string activityRuleId, CancellationToken cancellationToken);
}

internal sealed class DailyTrackerClientFacade(
    ILogger<DailyTrackerClientFacade> logger,
    IDailyTrackerHttpService httpService) : BaseFacade<DailyTrackerClientFacade>(logger), IDailyTrackerClientFacade
{
    public async Task<OneOf<bool, string>> CreateActivityFromActivityRuleAsync(string activityRuleId, CancellationToken cancellationToken)
    {
        var result = await httpService.CreateActivityFromActivityRuleAsync(activityRuleId, cancellationToken);

        return await HandleResponseAsync(result, cancellationToken);
    }
}