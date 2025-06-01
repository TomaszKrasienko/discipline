using System.Net;
using System.Net.Http.Json;
using discipline.ui.infrastructure.Clients;
using discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Requests;
using discipline.ui.infrastructure.Clients.DTOs.ActivityRules.Responses;
using discipline.ui.infrastructure.DTOs.ActivityRules;
using Microsoft.Extensions.Logging;
using OneOf;

namespace discipline.ui.infrastructure.Facades.ActivityRules.Facades;

public interface IActivityRuleClientFacade
{
    Task<OneOf<IReadOnlyList<ActivityRuleDto>, string>> BrowseAsync(CancellationToken cancellationToken);
    
    Task<OneOf<IReadOnlyList<ModeDto>, string>> BrowseModesAsync(CancellationToken cancellationToken);
    
    Task<OneOf<bool, string>> CreateAsync(
        WriteActivityRuleDto dto,
        CancellationToken cancellationToken);

    Task<OneOf<bool, string>> CreateActivityRuleStageAsync(
        WriteStageDto dto, 
        string activityRuleId, 
        CancellationToken cancellationToken);
    
    Task<OneOf<bool, string>> UpdateAsync(
        string activityRuleId,
        WriteActivityRuleDto dto,
        CancellationToken cancellationToken);
    
    Task<OneOf<bool, string>> DeleteAsync(
        string activityRuleId,
        CancellationToken cancellationToken);
    
    Task<OneOf<bool, string>> DeleteStageAsync(
        string activityRuleId,
        string stagesId,
        CancellationToken cancellationToken);
}

internal sealed class ActivityRuleClientFacade(
    ILogger<ActivityRuleClientFacade> logger,
    ICentreClient centreClient) : BaseFacade<ActivityRuleClientFacade>(logger), IActivityRuleClientFacade
{

    public async Task<OneOf<IReadOnlyList<ActivityRuleDto>, string>> BrowseAsync(CancellationToken cancellationToken)
    {
        var response = await centreClient.GetActivityRules(cancellationToken);
        
        var result = await HandleResponseAsync<IReadOnlyList<ActivityRuleResponseDto>>(
            response, cancellationToken);

        return result switch
        {
            { IsT0: true, AsT0: null } => new List<ActivityRuleDto>(),
            { IsT0: true, AsT0: not null } => result.AsT0.Select(ToActivityRuleDto).ToList(),
            _ => result.AsT1
        };
    }

    public async Task<OneOf<IReadOnlyList<ModeDto>, string>> BrowseModesAsync(CancellationToken cancellationToken)
    {
        var response = await centreClient.GetModesAsync(cancellationToken);

        var result = await HandleResponseAsync<IReadOnlyList<ModeResponseDto>>(
            response, cancellationToken);

        return result switch
        {
            { IsT0: true, AsT0: null } => new List<ModeDto>(),
            { IsT0: true, AsT0: not null } => result.AsT0.Select(x => new ModeDto(x.Mode, x.IsDaysRequired)).ToList(),
            _ => result.AsT1
        };
    }

    public async Task<OneOf<bool, string>> CreateAsync(WriteActivityRuleDto dto, CancellationToken cancellationToken)
    {
        var request = new CreateActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto(dto.Title, dto.Note),
            new ActivityRuleModeRequestDto(dto.Mode, dto.Days));
        
        var response = await centreClient.CreateActivityRule(request, cancellationToken);
        return await HandleResponseAsync(response, cancellationToken);
    }

    public async Task<OneOf<bool, string>> CreateActivityRuleStageAsync(WriteStageDto dto, string activityRuleId, CancellationToken cancellationToken)
    {
        var request = new CreateActivityRuleStageRequestDto(
            dto.Title);
        
        var response = await centreClient.CreateStage(activityRuleId, request, cancellationToken);
        return await HandleResponseAsync(response, cancellationToken);
    }
    
    public async Task<OneOf<bool, string>> UpdateAsync(string activityRuleId, WriteActivityRuleDto dto, CancellationToken cancellationToken)
    {
        var request = new UpdateActivityRuleRequestDto(
            new ActivityRuleDetailsRequestDto(dto.Title, dto.Note),
            new ActivityRuleModeRequestDto(dto.Mode, dto.Days));
        
        var response = await centreClient.UpdateActivityRule(activityRuleId, request, cancellationToken);
        return await HandleResponseAsync(response, cancellationToken);
    }

    public async Task<OneOf<bool, string>> DeleteAsync(string activityRuleId, CancellationToken cancellationToken)
    {
        var response = await centreClient.DeleteActivityRule(activityRuleId, cancellationToken);
        return await HandleResponseAsync(response, cancellationToken);
    }

    public async Task<OneOf<bool, string>> DeleteStageAsync(string activityRuleId, string stagesId, CancellationToken cancellationToken)
    {
        var response = await centreClient.DeleteActivityRuleStage(activityRuleId, stagesId, cancellationToken);
        return await HandleResponseAsync(response, cancellationToken);
    }

    private static ActivityRuleDto ToActivityRuleDto(ActivityRuleResponseDto response)
        => new(
            response.ActivityRuleId,
            new ActivityRuleDetailsDto(
                response.Details.Title,
                response.Details.Note),
            new ActivityRuleSelectedModeDto(
                response.Mode.Mode,
                response.Mode.Days),
            response.Stages.Select(ToStageDto).ToList());

    private static ActivityRuleStageDto ToStageDto(ActivityRuleStageResponseDto response)
        => new(
            response.StageId,
            response.Title,
            response.Index);
}