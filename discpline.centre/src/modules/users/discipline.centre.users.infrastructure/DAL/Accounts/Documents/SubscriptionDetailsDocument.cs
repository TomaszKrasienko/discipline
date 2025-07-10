namespace discipline.centre.users.infrastructure.DAL.Accounts.Documents;

public sealed class SubscriptionDetailsDocument
{    
    public required string Type { get; set; }
    public int? ValidityPeriod { get; set; }
    public bool RequirePayment { get; set; }
}