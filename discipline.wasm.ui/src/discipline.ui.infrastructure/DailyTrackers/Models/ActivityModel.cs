using discipline.ui.infrastructure.DailyTrackers.DTOs;

namespace discipline.ui.infrastructure.DailyTrackers.Models;

public sealed class ActivityModel
{
    private readonly HashSet<StageModel>? _stages;
    public string ActivityId { get; private set; }
    public string DailyTrackerId { get; private set; }
    public string Title { get; private set; }
    public string? Note { get; private set; }
    public bool IsChecked { get; private set; }
    public IReadOnlyCollection<StageModel>? Stages => _stages?.ToArray(); 

    private ActivityModel(string activityId, string dailyTrackerId, string title, string? note, bool isChecked,
        IEnumerable<StageModel>? stages)
    {
        ActivityId = activityId;
        DailyTrackerId = dailyTrackerId;
        Title = title;
        Note = note;
        IsChecked = isChecked;

        if (stages is not null)
        {
            _stages = new HashSet<StageModel>(stages);
        }
    }
    
    public static ActivityModel Create(string activityId, string dailyTrackerId, string title, string? note, bool isChecked,
        IEnumerable<StageModel>? stages)
        => new (activityId, dailyTrackerId, title, note, isChecked, stages);
    
    public void ChangeCheck()
        => IsChecked = !IsChecked;

    public void ChangeStageCheck(string stageId)
    {
        var stage = _stages?.Single(x => x.StageId == stageId);
        stage?.ChangeCheck();
    }

    public void ChangeStageIndex(int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex) return;

        var stage = Stages!.FirstOrDefault(s => s.Index == oldIndex);
        if (stage == null) return;

        if (oldIndex < newIndex)
        {
            foreach (var s in Stages!.Where(s => s.Index > oldIndex && s.Index <= newIndex))
            {
                s.DecreaseIndex();
            }
        }
        else
        {
            foreach (var s in Stages!.Where(s => s.Index >= newIndex && s.Index < oldIndex))
            {
                s.IncreaseIndex();
            }
        }
        
        stage.ChangeIndex(newIndex);
    }

    public void DeleteStage(string stageId)
    {
        var stage = Stages!.Single(s => s.StageId == stageId);
        _stages!.Remove(stage);

        foreach (var s in _stages.Select((stageModel, i) => (stageModel, i)))
        {
            s.stageModel.ChangeIndex(s.i + 1);
        }
    }
}