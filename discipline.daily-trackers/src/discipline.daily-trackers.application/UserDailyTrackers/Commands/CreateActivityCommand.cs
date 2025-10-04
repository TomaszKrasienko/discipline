using discipline.daily_trackers.domain.DailyTrackers.Services;
using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands;

public sealed record CreateActivityCommand(
    AccountId AccountId,
    Day Day,
    ActivityId ActivityId,
    ActivityDetailsSpecification ActivityDetails) : ICommand;
    
internal sealed class CreateActivityCommandHandler(
    IUserDailyTrackerFactory dailyTrackerFactory) : ICommandHandler<CreateActivityCommand>
{
    public Task HandleAsync(CreateActivityCommand command, CancellationToken cancellationToken)
        => dailyTrackerFactory.Create(
            command.AccountId,
            command.Day,
            command.ActivityId,
            command.ActivityDetails, 
            cancellationToken);
}