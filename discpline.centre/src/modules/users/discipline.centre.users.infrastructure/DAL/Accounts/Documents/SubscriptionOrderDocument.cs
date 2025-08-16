namespace discipline.centre.users.infrastructure.DAL.Accounts.Documents;

internal sealed class SubscriptionOrderDocument
{
    public required string Id { get; set; }
    public required IntervalDocument Interval { get; set; }
    public required SubscriptionDetailsDocument SubscriptionDetails { get; set; }
    public required PaymentDocument? Payment { get; set; }
    public required string SubscriptionId { get; set; }
}