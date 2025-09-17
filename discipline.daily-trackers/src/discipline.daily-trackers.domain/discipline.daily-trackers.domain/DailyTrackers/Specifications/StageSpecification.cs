using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers.Specifications;

public sealed record StageSpecification(StageId StageId, string Title, int Index);