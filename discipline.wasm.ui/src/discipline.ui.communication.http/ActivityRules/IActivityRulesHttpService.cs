using discipline.ui.communication.http.ActivityRules.DTOs.Requests;
using Refit;

namespace discipline.ui.communication.http.ActivityRules;

public interface IActivityRulesHttpService
{
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
}