namespace discipline.ui.communication.http.DailyTrackers.Requests;

public record CreateActivityRequestDto(DateOnly Day, ActivityDetailsRequestDto Details,
    List<CreateStageRequestDto>? Stages);