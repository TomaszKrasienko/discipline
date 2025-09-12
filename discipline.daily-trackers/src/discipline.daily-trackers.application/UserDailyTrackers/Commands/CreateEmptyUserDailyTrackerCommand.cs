using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Services;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.Abstractions.Commands;
using discipline.libs.exceptions.Exceptions;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands;

public sealed record CreateEmptyUserDailyTrackerCommand(
    DailyTrackerId Id,
    AccountId AccountId,
    DateOnly Day) : ICommand;
    
public sealed class CreateEmptyUserDailyTrackerCommandHandler(
    IUserDailyTrackerFactory userDailyTrackerFactory,
    IReadWriteUserDailyTrackerRepository userDailyTrackerRepository) : ICommandHandler<CreateEmptyUserDailyTrackerCommand>
{
    public async Task HandleAsync(CreateEmptyUserDailyTrackerCommand command, CancellationToken cancellationToken)
    {
        if (await userDailyTrackerRepository.DoesExistAsync(
                command.AccountId,
                command.Day,
                cancellationToken))
        {
            throw new DisciplineNotUniqueException("CreateEmptyUserDailyTracker.NotUnique", command.AccountId.ToString(), command.Day.ToString("dd/MM/yyyy"));
        }

        await userDailyTrackerFactory.Create(
            command.Id,
            command.AccountId,
            command.Day,
            cancellationToken);
    }
}