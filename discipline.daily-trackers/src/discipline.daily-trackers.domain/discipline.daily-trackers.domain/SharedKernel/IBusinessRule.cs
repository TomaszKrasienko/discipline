namespace discipline.daily_trackers.domain.SharedKernel;

public interface IBusinessRule
{
    public Exception Exception { get; }
    bool IsBroken();
}