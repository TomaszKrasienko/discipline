using System.Net;
using System.Net.Http.Json;
using discipline.ui.communication.http.DailyTrackers;
using discipline.ui.communication.http.DailyTrackers.Responses;
using discipline.ui.infrastructure.DailyTrackers.DTOs;
using discipline.ui.infrastructure.DailyTrackers.Models;
using Microsoft.AspNetCore.Components;
using OneOf;
using Refit;

namespace discipline.ui.infrastructure.DailyTrackers.DailyTrackers;

public interface IBrowseDailyTrackerFacade
{
    Task<OneOf<DailyTrackerDto, string>> HandleAsync(DateOnly day, CancellationToken cancellationToken);
}

internal sealed class BrowseDailyTrackerFacade(
    IDailyTrackerHttpService dailyTrackerHttpService,
    NavigationManager navigationManager) : IBrowseDailyTrackerFacade
{
    public async Task<OneOf<DailyTrackerDto, string>> HandleAsync(DateOnly day, CancellationToken cancellationToken)
    {
        var getDailyTrackerResponse = await dailyTrackerHttpService.GetDailyTrackerByDayAsync(day.ToString("yyyy-MM-dd"), cancellationToken);

        if (getDailyTrackerResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            navigationManager.NavigateTo("/sign-in");
        }

        if (getDailyTrackerResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return DailyTrackerDto.Create(day, []);
        }
        
        if (!getDailyTrackerResponse.IsSuccessStatusCode)
        {
            var response = await getDailyTrackerResponse.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
            return response?.Detail ?? "Unknown Error";
        }
        
        var dailyTrackerResponse = await getDailyTrackerResponse.Content.ReadFromJsonAsync<DailyTrackerResponseDto>(cancellationToken);

        if (dailyTrackerResponse is null)
        {
            return "Unknown Error";
        }

        List<ActivityModel> activities = [];
        
        foreach (var activity in dailyTrackerResponse.Activities)
        {
            var stages = activity.Stages?.Select(x => StageModel.Create(x.StageId, x.Title, x.Index, x.IsChecked));
            activities.Add(ActivityModel.Create(activity.ActivityId, dailyTrackerResponse.DailyTrackerId, activity.Details.Title, activity.Details.Note,
                activity.IsChecked, stages));
        }

        var dailyTracker = DailyTrackerDto.Create(dailyTrackerResponse.Day, activities);

        return dailyTracker;
    }
}