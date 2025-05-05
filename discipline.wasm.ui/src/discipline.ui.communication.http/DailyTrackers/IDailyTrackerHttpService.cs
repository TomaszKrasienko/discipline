using discipline.ui.communication.http.DailyTrackers.Requests;
using Refit;

namespace discipline.ui.communication.http.DailyTrackers;

public interface IDailyTrackerHttpService
{
    [Get("/api/daily-trackers-module/daily-trackers/{day}")]
    public Task<HttpResponseMessage> GetDailyTrackerByDayAsync(string day, CancellationToken cancellationToken);

    [Post("/api/daily-trackers-module/daily-trackers/activities")]
    public Task<HttpResponseMessage> CreateActivityAsync(CreateActivityRequestDto request, CancellationToken cancellationToken);
    
    [Patch("/api/daily-trackers/{dailyTrackerId}/activities/{activityId}")]
    public Task<HttpResponseMessage> ChangeActivityCheckAsync(string dailyTrackerId, string activityId, CancellationToken cancellationToken);
    
    [Patch("/api/daily-trackers/{dailyTrackerId}/activities/{activityId}/stages/{stageId}")]
    public Task<HttpResponseMessage> ChangeActivityStageCheckAsync(string dailyTrackerId, string activityId, string stageId, 
        CancellationToken cancellationToken);
}