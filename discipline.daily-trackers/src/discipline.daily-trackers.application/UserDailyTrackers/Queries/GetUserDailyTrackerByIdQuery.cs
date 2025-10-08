using discipline.daily_trackers.application.UserDailyTrackers.DTOs.Responses;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.daily_trackers.application.UserDailyTrackers.Queries;

public sealed record GetUserDailyTrackerByIdQuery(
    AccountId AccountId,
    DailyTrackerId UserDailyTrackerId) : IQuery<UserDailyTrackerResponseDto?>;
