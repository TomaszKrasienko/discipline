using discipline.libs.cqrs.Abstractions.Commands;

namespace discipline.hangfire.create_empty_daily_tracker.Commands;

public sealed record CreateEmptyUserDailyTracker(
    string AccountId,
    DateOnly Day) : ICommand;