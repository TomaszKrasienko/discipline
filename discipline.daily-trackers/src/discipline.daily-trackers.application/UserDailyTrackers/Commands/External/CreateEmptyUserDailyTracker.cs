using discipline.libs.cqrs.Abstractions.Commands;

namespace discipline.daily_trackers.application.UserDailyTrackers.Commands.External;

public sealed record CreateEmptyUserDailyTracker(
    string AccountId,
    DateOnly Day) : ICommand;