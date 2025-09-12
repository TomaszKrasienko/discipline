using discipline.centre.dailytrackers.domain;
using discipline.centre.dailytrackers.domain.Repositories;
using discipline.centre.shared.abstractions.CQRS.Commands;
using discipline.centre.shared.abstractions.Exceptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Commands;

public sealed record CreateEmptyDailyTrackerCommand(
    DailyTrackerId Id,
    DateOnly Day,
    AccountId AccountId) : ICommand;

internal sealed class CreateEmptyDailyTrackerCommandHandler(
    IWriteDailyTrackerRepository dailyTrackerRepository) : ICommandHandler<CreateEmptyDailyTrackerCommand>
{
    public async Task HandleAsync(CreateEmptyDailyTrackerCommand command, CancellationToken cancellationToken)
    {
        var doesDailyTrackerExist = await dailyTrackerRepository
            .ExistsAsync(
                command.AccountId,
                command.Day,
                cancellationToken);

        if (doesDailyTrackerExist)
        {
            throw new NotUniqueException(
                "CreateEmptyDailyTracker.DailyTrackerNotUnique",
                command.Day.ToString());
        }

        var dailyTracker = DailyTracker.Create(
            command.Id,
            command.Day,
            command.AccountId);
        
        await dailyTrackerRepository.AddAsync(dailyTracker, cancellationToken);
    }
}