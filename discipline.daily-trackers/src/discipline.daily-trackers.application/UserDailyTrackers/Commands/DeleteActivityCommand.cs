using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands;

public sealed record DeleteActivityCommand(
    AccountId AccountId,
    DailyTrackerId DailyTrackerId,
    ActivityId ActivityId) : ICommand;
    
internal sealed class DeleteActivityCommandHandler(
    IReadWriteUserDailyTrackerRepository userDailyTrackerRepository) : ICommandHandler<DeleteActivityCommand>
{
    public async Task HandleAsync(DeleteActivityCommand command, CancellationToken cancellationToken)
    {
        var userDailyTracker = await userDailyTrackerRepository
            .GetByIdAsync(
                command.AccountId,
                command.DailyTrackerId,
                cancellationToken);

        if (userDailyTracker is null)
        {
            return;
        }
        
        userDailyTracker.RemoveActivity(command.ActivityId);
        
        await userDailyTrackerRepository.UpdateAsync(
            userDailyTracker,
            cancellationToken);    
    }
}