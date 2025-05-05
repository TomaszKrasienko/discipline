using OneOf;

namespace discipline.ui.infrastructure.DailyTrackers.Activities;

public interface IDeleteActivityFacade
{
    Task<OneOf<bool, string>> HandleAsync(string dailyTrackerId, string activityId,
        CancellationToken cancellationToken);
}

internal sealed class DeleteActivityFacade : IDeleteActivityFacade
{
    public async Task<OneOf<bool, string>> HandleAsync(string dailyTrackerId, string activityId, CancellationToken cancellationToken)
    {   
        await Task.Delay(1, cancellationToken);
        return true;
    }
}