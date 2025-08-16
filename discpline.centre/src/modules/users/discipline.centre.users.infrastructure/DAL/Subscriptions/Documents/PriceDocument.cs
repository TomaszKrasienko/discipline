using discipline.centre.shared.infrastructure.DAL;

namespace discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;

internal sealed class PriceDocument : IDocument
{
    public decimal PerMonth { get; set; }
    public decimal PerYear { get; set; }
    public required string Currency { get; set; }
}