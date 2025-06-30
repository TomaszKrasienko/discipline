using discipline.centre.shared.abstractions.SharedKernel;

namespace discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

public sealed class Interval : ValueObject
{
    public DateOnly StartDate { get; set; }
    public DateOnly? FinishDate { get; set; }

    private Interval(DateOnly startDate, DateOnly? finishDate)
    {
        StartDate = startDate;
        FinishDate = finishDate;
    }
    
    internal static Interval Create(
        DateOnly startDate,
        DateOnly? finishDate) 
        => new(startDate, finishDate);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StartDate;
        yield return FinishDate;
    }
}