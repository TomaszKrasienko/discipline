using System.Net.Http.Json;
using discipline.ui.communication.http.DailyTrackers;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.DailyTrackers.Stages;

public interface IChangeActivityStageCheckFacade
{
    Task<OneOf<bool, string>> HandleAsync(string disciplineId, string activityId, string stageId, CancellationToken cancellationToken);
}

internal sealed class ChangeActivityStageCheckFacade(
    IDailyTrackerHttpService dailyTrackerHttpService) : IChangeActivityStageCheckFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(string disciplineId, string activityId,
        string stageId, CancellationToken cancellationToken)
    {
        var response = await dailyTrackerHttpService.ChangeActivityStageCheckAsync(disciplineId, activityId, stageId, cancellationToken); 
        
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        var errorResult = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
        return errorResult?.Detail ?? "Unexpected error";
    }
}