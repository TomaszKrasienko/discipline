using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Services;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.Abstractions.Commands;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands;

public sealed record CreateActivityCommand(
    AccountId AccountId,
    ActivityId ActivityId,
    DateOnly Day,
    ActivityDetailsSpecification Details) : ICommand;

internal sealed class CreateActivityCommandHandler(
    IReadWriteUserDailyTrackerRepository writeReadDailyTrackerRepository,
    IUserDailyTrackerFactory dailyTrackerFactory) : ICommandHandler<CreateActivityCommand>
{
    public async Task HandleAsync(CreateActivityCommand command, CancellationToken cancellationToken = default)
    {
        var dailyTracker = await writeReadDailyTrackerRepository
            .GetByDayAsync(
                command.AccountId,
                command.Day,
                cancellationToken);

        if (dailyTracker is null)
        {
            dailyTracker = await dailyTrackerFactory.Create(
                DailyTrackerId.New(), 
                command.AccountId,
                command.Day,
                command.ActivityId, 
                command.Details,
                null,
                cancellationToken);
            
            return;
        }
        
        dailyTracker.AddActivity(command.ActivityId, command.Details, null);
        
        await writeReadDailyTrackerRepository.UpdateAsync(dailyTracker, cancellationToken);
    }
}