using discipline.daily_trackers.application.UserDailyTrackers.DTOs.ExternalCommands;
using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands.External;

public sealed record CreateActivitiesFromActivityRulesCommand(
    string AccountId,
    DateOnly Day,
    IReadOnlyCollection<CreateActivityDto> Activities) : ICommand;