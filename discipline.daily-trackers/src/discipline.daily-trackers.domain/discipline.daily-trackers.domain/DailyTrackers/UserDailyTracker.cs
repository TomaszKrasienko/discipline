using discipline.daily_trackers.domain.DailyTrackers.Specifications;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.DailyTrackers;
using discipline.daily_trackers.domain.SharedKernel.Aggregate;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers;

public sealed class UserDailyTracker : AggregateRoot<DailyTrackerId, Ulid>
{
    private readonly List<Activity> _activities = [];
    public AccountId AccountId { get; private set; }
    public Day Day { get; private set; }
    public DailyTrackerId? Next { get; private set; }
    public DailyTrackerId? Prior { get; private set; }
    public IReadOnlyCollection<Activity> Activities => _activities;

     /// <summary>
     /// <remarks>Use only for Mongo purposes</remarks>
     /// </summary>
     public UserDailyTracker(
         DailyTrackerId id,
         AccountId accountId,
         Day day,
         DailyTrackerId? next,
         DailyTrackerId? prior,
         List<Activity> activities) : this(
             id,
             accountId,
             day, 
             prior)
         => _activities = activities;

     private UserDailyTracker(
         DailyTrackerId id,
         AccountId accountId,
         Day day,
         DailyTrackerId? prior) : base(id)
     {
         AccountId = accountId;
         Day = day;
         Prior = prior;
     }

     internal static UserDailyTracker Create(
         DailyTrackerId id,
         AccountId accountId,
         DateOnly day,
         ActivityId activityId,
         ActivityDetailsSpecification details,
         ActivityRuleId? parentActivityRuleId,
         DailyTrackerId? prior)
     {
         var dailyTracker = new UserDailyTracker(id, accountId, day, prior);
         dailyTracker.AddActivity(activityId, details, parentActivityRuleId);
         return dailyTracker;
     }

     internal static UserDailyTracker Create(
         DailyTrackerId id,
         AccountId accountId,
         DateOnly day,
         DailyTrackerId? prior)
     {
         var dailyTracker = new UserDailyTracker(id, accountId, day, prior);
         return dailyTracker;
     }
     
     internal void SetNext(DailyTrackerId next)
        => Next = next;

     public Activity AddActivity(
         ActivityId activityId,
         ActivityDetailsSpecification details,
         ActivityRuleId? parentActivityRuleId)
     {
         if (_activities.Exists(x => x.Details.Title == details.Title))
         {
             throw new DomainException("DailyTracker.Activity.Title.AlreadyExists");
         }

         Activity activity;

         if (parentActivityRuleId is null)
         {
             activity = Activity.Create(
                 activityId,
                 details);    
         }
         else
         {
             activity = Activity.CreateFromRule(
                 activityId,
                 parentActivityRuleId.Value,
                 details);
         }
         
         _activities.Add(activity);
         return activity;
     }
     
//     public void MarkActivityAsChecked(ActivityId activityId)
//     {
//         var activity = _activities.SingleOrDefault(x => x.Id == activityId);
//     
//         if (activity is null)
//         {
//             throw new DomainException("DailyTracker.Activity.NotExists");
//         }
//         
//         activity.MarkAsChecked();
//     }
//     
//     public bool DeleteActivity(ActivityId activityId)
//     {
//         var activity = _activities.SingleOrDefault(x => x.Id == activityId);
//     
//         if (activity is null)
//         {
//             return false;
//         }
//         
//         _activities.Remove(activity);
//         return true;
//     }
//     
//     public void MarkActivityStageAsChecked(ActivityId activityId, StageId stageId)
//     {
//         var activity = _activities.SingleOrDefault(x => x.Id == activityId);
//         
//         if (activity is null)
//         {
//             throw new DomainException("DailyTracker.Activity.NotFound",
//                 $"Activity with id {activityId} not found.");    
//         }
//         
//         activity.MarkStageAsChecked(stageId);
//     }
//     
//     public bool DeleteActivityStage(ActivityId activityId, StageId stageId)
//     {
//         var activity = _activities.SingleOrDefault(x => x.Id == activityId);
//     
//         if (activity is null)
//         {
//             return false;
//         }
//         
//         return activity.DeleteStage(stageId);
//     }
//     
//     public void ClearParentActivityRuleIdIs(ActivityRuleId parentActivityRuleId)
//     {
//         foreach (var activity in _activities.Where(x => x.ParentActivityRuleId == parentActivityRuleId))
//         {
//             activity.ClearParentActivityRuleId();
//         }
//     }
}