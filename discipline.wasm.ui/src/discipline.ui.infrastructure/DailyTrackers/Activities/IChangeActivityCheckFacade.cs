using System.Net.Http.Json;
using discipline.ui.communication.http.DailyTrackers;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.DailyTrackers.Activities;

public interface IChangeActivityCheckFacade
{
    Task<OneOf<bool, string>> HandleAsync(string dailyTrackerId, string activityId,
        CancellationToken cancellationToken);
}

internal sealed class ChangeActivityCheckFacade(
    IDailyTrackerHttpService dailyTrackerHttpService) : IChangeActivityCheckFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(string dailyTrackerId, string activityId, CancellationToken cancellationToken)
    {
        var response = await dailyTrackerHttpService.ChangeActivityCheckAsync(dailyTrackerId, activityId, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        var errorResult = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
        return errorResult?.Detail ?? "Unexpected error";

    }
}