using discipline.daily_trackers.domain.DailyTrackers.ValueObjects;
using discipline.daily_trackers.domain.DailyTrackers.ValueObjects.Stages;
using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

namespace discipline.daily_trackers.domain.DailyTrackers;

public sealed class Stage : Entity<StageId, Ulid>
{
    public Title Title { get; private set; }
    
    public OrderIndex Index { get; private set; }
    
    public IsChecked IsChecked { get; private set; }
    
    public Stage(StageId stageId, Title title, OrderIndex index, IsChecked isChecked) : base(stageId)
    {
        Title = title;
        Index = index;
        IsChecked = isChecked;
    }
    
    internal static Stage Create(StageId stageId, string title, int index)
        => new (stageId, title, index, false);
    
    internal void MarkAsChecked()
        => IsChecked = true;
    
    internal void ChangeIndex(int index)
        => Index = index;
}