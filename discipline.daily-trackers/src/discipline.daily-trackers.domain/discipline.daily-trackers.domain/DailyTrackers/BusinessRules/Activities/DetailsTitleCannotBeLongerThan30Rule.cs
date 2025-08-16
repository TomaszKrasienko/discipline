using discipline.daily_trackers.domain.SharedKernel;
using discipline.daily_trackers.domain.SharedKernel.Exceptions;

namespace discipline.daily_trackers.domain.DailyTrackers.BusinessRules.Activities;

internal sealed class DetailsTitleCannotBeLongerThan30Rule(string value) : IBusinessRule
{
    public Exception Exception => new DomainException("DailyTracker.Activity.Details.Title.TooLong",
        $"Title: {value} is longer than 30.");
    public bool IsBroken()
        => value.Length > 30;
}