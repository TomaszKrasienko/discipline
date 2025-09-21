using discipline.daily_trackers.domain.DailyTrackers;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Activities;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

// ReSharper disable once CheckNamespace
namespace discipline.daily_trackers.infrastructure.DAL.DailyTrackers.Documents;

//TODO: Unit tests
internal static class UserDailyTrackerDocumentMapperExtensions
{
    internal static UserDailyTracker ToEntity(this UserDailyTrackerDocument document)
        => new(
            DailyTrackerId.Parse(document.Id),
            AccountId.Parse(document.AccountId),
            Day.Create(document.Day),
            document.Next is null ? null : DailyTrackerId.Parse(document.Next),
            document.Prior is null ? null : DailyTrackerId.Parse(document.Prior),
            document.Activities.Select(x => x.ToEntity()).ToList());

    private static Activity ToEntity(this ActivityDocument document)
        => new(
            ActivityId.Parse(document.Id),
            Details.Create(document.Details.Title, document.Details.Note),
            document.IsChecked,
            document.ParentActivityRuleId is null ? null : ActivityRuleId.Parse(document.ParentActivityRuleId),
            document.Stages.Select(x => x.ToEntity()).ToHashSet());

    private static Stage ToEntity(this StageDocument document)
        => new(
            StageId.Parse(document.Id),
            document.Title,
            document.Index,
            document.IsChecked);
}