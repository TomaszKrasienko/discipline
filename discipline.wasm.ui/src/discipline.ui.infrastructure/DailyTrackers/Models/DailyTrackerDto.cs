using discipline.ui.infrastructure.DailyTrackers.Models;

namespace discipline.ui.infrastructure.DailyTrackers.DTOs;

public sealed class DailyTrackerDto
{
    private readonly HashSet<ActivityModel> _activities;
    public DateOnly Day { get; private set; }
    public IReadOnlyCollection<ActivityModel> Activities => _activities.ToArray();

    private DailyTrackerDto(DateOnly day, IEnumerable<ActivityModel> activities)
    {
        Day = day;
        _activities = new HashSet<ActivityModel>(activities);
    }
    
    public static DailyTrackerDto Create(DateOnly day, IEnumerable<ActivityModel> activities)
        => new (day, activities);
}