using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Accounts.Rules.Intervals;

namespace discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

public sealed class Interval : ValueObject
{
    public DateOnly StartDate { get; }
    public DateOnly? PlannedFinishDate { get; }
    public DateOnly? FinishDate { get; }

    private Interval(
        DateOnly startDate,
        DateOnly? plannedFinishDate,
        DateOnly? finishDate)
    {
        StartDate = startDate;
        PlannedFinishDate = plannedFinishDate;
        FinishDate = finishDate;
    }

    public static Interval Create(
        DateOnly startDate,
        DateOnly? plannedFinishDate,
        DateOnly? finishDate)
    {
        if (plannedFinishDate is not null)
        {
            CheckRule(new FinishDateBeforeStartDateRule(startDate, plannedFinishDate.Value, nameof(PlannedFinishDate)));
        }
        
        if (finishDate is not null)
        {
            CheckRule(new FinishDateBeforeStartDateRule(startDate, finishDate.Value, nameof(FinishDate)));
        }

        return new(startDate, plannedFinishDate, finishDate);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StartDate;
        yield return PlannedFinishDate;
        yield return FinishDate;
    }
}