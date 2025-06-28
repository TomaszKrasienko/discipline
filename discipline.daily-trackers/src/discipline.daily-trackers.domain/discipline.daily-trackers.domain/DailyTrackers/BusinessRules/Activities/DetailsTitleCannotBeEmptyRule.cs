using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Activities;

internal sealed class DetailsTitleCannotBeEmptyRule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Activity.Details.Title.Empty",
        "Activity title cannot be empty.");

    public bool IsBroken()
        => string.IsNullOrWhiteSpace(value);
}