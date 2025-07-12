namespace discipline.centre.users.infrastructure.DAL.Accounts.Documents;

internal sealed class AccountDocument
{
    public required string Id { get; set; }
    public required string Login { get; set; }
    public required string HashedPassword { get; set; }
    public required List<SubscriptionOrderDocument> SubscriptionOrders { get; set; }
}