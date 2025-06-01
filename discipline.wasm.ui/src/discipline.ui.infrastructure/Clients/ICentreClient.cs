using discipline.ui.communication.http.DailyTrackers.Requests;
using discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Requests;
using discipline.ui.infrastructure.Clients.DTOs.Users.Requests;
using Refit;

namespace discipline.ui.infrastructure.Clients;

internal interface ICentreClient
{
    // Users
    [Post("/api/users-module/users/tokens")]
    Task<HttpResponseMessage> SignIn(SignInRequestDto signInRequestDto, CancellationToken cancellationToken);
    
    // Activity rules
    [Get("/api/activity-rules")]
    public Task<HttpResponseMessage> GetActivityRules(CancellationToken cancellationToken);
    
    [Get("/api/activity-rules/modes")]
    public Task<HttpResponseMessage> GetModesAsync(CancellationToken cancellationToken);
    
    [Delete("/api/activity-rules/{id}")]
    public Task<HttpResponseMessage> DeleteActivityRule(string id, CancellationToken cancellationToken);
    
    [Delete("/api/activity-rules/{activityRuleId}/stages/{stagesId}")]
    public Task<HttpResponseMessage> DeleteActivityRuleStage(string activityRuleId, string stagesId, CancellationToken cancellationToken);
    
    [Post("/api/activity-rules")]
    public Task<HttpResponseMessage> CreateActivityRule(CreateActivityRuleRequestDto request, CancellationToken cancellationToken);

    [Post("/api/activity-rules/{activityRuleId}/stages")]
    public Task<HttpResponseMessage> CreateStage(string activityRuleId, CreateActivityRuleStageRequestDto dto, CancellationToken cancellationToken);
    
    [Put("/api/activity-rules/{activityRuleId}")]
    public Task<HttpResponseMessage> UpdateActivityRule(string activityRuleId, 
        UpdateActivityRuleRequestDto request,
        CancellationToken cancellationToken);
    
    // Daily trackers
    [Get("/api/daily-trackers-module/daily-trackers/{day}")]
    public Task<HttpResponseMessage> GetDailyTrackerByDayAsync(string day, CancellationToken cancellationToken);

    [Post("/api/daily-trackers-module/daily-trackers/activities")]
    public Task<HttpResponseMessage> CreateActivityAsync(CreateActivityRequestDto request, CancellationToken cancellationToken);
    
    [Patch("/api/daily-trackers/{dailyTrackerId}/activities/{activityId}")]
    public Task<HttpResponseMessage> ChangeActivityCheckAsync(string dailyTrackerId, string activityId, CancellationToken cancellationToken);
    
    [Patch("/api/daily-trackers/{dailyTrackerId}/activities/{activityId}/stages/{stageId}")]
    public Task<HttpResponseMessage> ChangeActivityStageCheckAsync(string dailyTrackerId, string activityId, string stageId, 
        CancellationToken cancellationToken);
    
    [Post("/api/daily-trackers/activities/activity-rules/{activityRuleId}")]
    public Task<HttpResponseMessage> CreateActivityFromActivityRuleAsync(string activityRuleId, CancellationToken cancellationToken);
}