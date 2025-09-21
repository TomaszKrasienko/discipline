using discipline.libs.cqrs.abstractions.Commands;

namespace discipline.activity_scheduler.create_empty_daily_tracker.Commands;

public sealed record CreateEmptyUserDailyTrackerCommand(
    string AccountId,
    DateOnly Day) : ICommand;