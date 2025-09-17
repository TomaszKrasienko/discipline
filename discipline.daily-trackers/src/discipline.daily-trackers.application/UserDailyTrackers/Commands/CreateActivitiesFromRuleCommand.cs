using discipline.daily_trackers.domain.DailyTrackers.Repositories;
using discipline.daily_trackers.domain.DailyTrackers.Services;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;
using discipline.libs.exceptions.Exceptions;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands;

public sealed record CreateActivitiesFromRulesCommand(
    DailyTrackerId Id,
    AccountId AccountId,
    DateOnly Day,
    IReadOnlyCollection<ActivitySpecification> Activities) : ICommand;
    
internal sealed class CreateActivitiesFromRuleCommandHandler(
    IReadWriteUserDailyTrackerRepository userDailyTrackerRepository,
    IUserDailyTrackerFactory factory) : ICommandHandler<CreateActivitiesFromRulesCommand>
{
    public async Task HandleAsync(CreateActivitiesFromRulesCommand command, CancellationToken cancellationToken)
    {
        var doesExists = await userDailyTrackerRepository
            .DoesExistAsync(
                command.AccountId,
                command.Day,
                cancellationToken);

        if (doesExists)
        {
            throw new DisciplineNotUniqueException(
                "CreateActivitiesFromRuleCommandHandler.NotUnique",
                command.AccountId.ToString(),
                command.Day.ToString());
        }

        await factory.Create(
            command.Id,
            command.AccountId,
            command.Day,
            command.Activities,
            cancellationToken);
    }
}