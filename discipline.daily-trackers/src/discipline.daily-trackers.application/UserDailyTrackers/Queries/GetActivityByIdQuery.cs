using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.application.UserDailyTrackers.Queries;

public sealed record GetActivityByIdQuery(AccountId AccountId, ActivityId ActivityId);