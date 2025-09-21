using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;

namespace discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Mappers;

//TODO: Unit tests
internal static class UserDailyTrackerMapperExtensions
{
    internal static UserDailyTrackerDocument ToDocument(this UserDailyTracker entity)
        => new()
        {
            Id = entity.Id.ToString(),
            AccountId = entity.AccountId.ToString(),
            Day = entity.Day,
            Next = entity.Next?.ToString(),
            Prior = entity.Prior?.ToString(),
            Activities = entity.Activities.Select(x => x.ToDocument()).ToList()
        };

    private static ActivityDocument ToDocument(this Activity entity)
        => new()
        {
            Id = entity.Id.ToString(),
            Details = new()
            {
                Title = entity.Details.Title,
                Note = entity.Details.Note
            },
            IsChecked = entity.IsChecked,
            ParentActivityRuleId = entity.ParentActivityRuleId?.ToString(),
            Stages = entity.Stages.Select(x => x.ToDocument()).ToList()
        };

    private static StageDocument ToDocument(this Stage entity)
        => new()
        {
            Id = entity.Id.ToString(),
            Title = entity.Title,
            Index = entity.Index,
            IsChecked = entity.IsChecked
        };
}