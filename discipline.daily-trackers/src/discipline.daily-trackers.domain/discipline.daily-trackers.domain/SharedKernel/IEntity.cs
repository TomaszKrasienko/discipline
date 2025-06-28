namespace discipline.daily_trackers.domain.SharedKernel;

public interface IEntity
{
    public int Version { get; }
    void IncreaseVersion();
}