namespace discipline.centre.users.infrastructure.DAL.Accounts.Documents;

internal sealed class IntervalDocument
{
    public DateOnly StartDate { get; set; }
    public DateOnly? FinishDate { get; set; }
}