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
             next,
             prior)
         => _activities = activities;

     private UserDailyTracker(
         DailyTrackerId id,
         AccountId accountId,
         Day day,
         DailyTrackerId? next,
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
         DailyTrackerId? prior)
     {
         var dailyTracker = new UserDailyTracker(id, accountId, day, null, prior);
         return dailyTracker;
     }

     internal static UserDailyTracker Create(
         DailyTrackerId id,
         AccountId accountId,
         DateOnly day,
         DailyTrackerId? prior,
         IReadOnlyCollection<ActivitySpecification> activities)
     {
         var userDailyTracker = Create(id, accountId, day, prior);

         foreach (var activity in activities)
         {
             userDailyTracker.AddActivity(
                 activity.ActivityId,
                 activity.Details,
                 activity.ParentActivityRuleId,
                 activity.Stages);
         }
         
         return userDailyTracker;
     }
     
     internal void SetNext(DailyTrackerId next)
        => Next = next;

     private Activity AddActivity(
         ActivityId activityId,
         ActivityDetailsSpecification details,
         ActivityRuleId parentActivityRuleId,
         IReadOnlyCollection<StageSpecification> stages)
     {
         if (_activities.Exists(x => x.Details.Title == details.Title))
         {
             throw new DomainException("DailyTracker.Activity.Title.AlreadyExists");
         }
         
         var activity = Activity.CreateFromRule(
                 activityId,
                 parentActivityRuleId,
                 details,
                 stages);
         
         _activities.Add(activity);
         return activity;
     }
}