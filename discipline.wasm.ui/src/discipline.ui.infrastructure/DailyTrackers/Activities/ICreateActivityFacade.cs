using System.Net.Http.Json;
using discipline.ui.communication.http.DailyTrackers;
using discipline.ui.communication.http.DailyTrackers.Requests;
using discipline.ui.infrastructure.DailyTrackers.DTOs;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.DailyTrackers.Activities;

public interface ICreateActivityFacade
{
    Task<OneOf<bool, string>> HandleAsync(DateOnly day, string title, string? note, List<StageModel>? stages,
        CancellationToken cancellationToken);
}

internal sealed class CreateActivityFacade(
    IDailyTrackerHttpService dailyTrackerHttpService) : ICreateActivityFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(DateOnly day, string title, string? note, List<StageModel>? stages,
        CancellationToken cancellationToken)
    {
        List<CreateStageRequestDto>? createStageRequests = null;
        
        if (stages is not null && stages.Any())
        {
            createStageRequests = stages.Select(s => new CreateStageRequestDto(s.Title, s.Index)).ToList();
        }

        var request = new CreateActivityRequestDto(day, new ActivityDetailsRequestDto(title, note),
            createStageRequests);

        var response = await dailyTrackerHttpService.CreateActivityAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var details = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
            return details?.Detail ?? "Unknown Error";
        }

        return true;
    }
}