using discipline.centre.dailytrackers.application.DailyTrackers.DTOs.Responses;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.libs.cqrs.abstractions.Queries;

namespace discipline.centre.dailytrackers.application.DailyTrackers.Queries;

public sealed record GetActivityByIdQuery(AccountId AccountId, ActivityId ActivityId) : IQuery<ActivityDto?>;
