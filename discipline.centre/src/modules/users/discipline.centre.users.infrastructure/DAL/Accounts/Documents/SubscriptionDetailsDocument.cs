namespace discipline.centre.users.infrastructure.DAL.Accounts.Documents;

internal sealed class SubscriptionDetailsDocument
{
    public required string Type { get; set; }
    public string? ValidityPeriod { get; set; }
    public bool RequirePayment { get; set; }
}