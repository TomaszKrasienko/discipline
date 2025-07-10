namespace discipline.centre.users.infrastructure.DAL.Accounts.Documents;

public sealed class PaymentDocument
{
    public decimal Value { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}