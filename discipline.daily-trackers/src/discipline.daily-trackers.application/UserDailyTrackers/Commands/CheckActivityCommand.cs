using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;
using discipline.libs.exceptions.Exceptions;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands;

public sealed record CheckActivityCommand(
    AccountId AccountId,
    DailyTrackerId UserDailyTrackerId,
    ActivityId ActivityId) : ICommand;
    
    
internal sealed class CheckActivityCommandHandler(
    IReadWriteUserDailyTrackerRepository readWriteUserDailyTrackerRepository) : ICommandHandler<CheckActivityCommand>
{
    public async Task HandleAsync(CheckActivityCommand command, CancellationToken cancellationToken)
    {
        var userDailyTracker = await readWriteUserDailyTrackerRepository
            .GetByIdAsync(
                command.AccountId,
                command.UserDailyTrackerId,
                cancellationToken);

        if (userDailyTracker is null)
        {
            throw new DisciplineNotFoundException(
                nameof(CheckActivityCommandHandler),
                nameof(UserDailyTracker),
                command.UserDailyTrackerId.ToString());
        }
    }
}