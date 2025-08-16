using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Stages;

internal sealed class TitleCanNotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Stage.Title.Empty",
        "Activity rule title can not be empty");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}