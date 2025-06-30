namespace discipline.centre.users.domain.Subscriptions.Enums;

public sealed record SubscriptionType
{
    public static readonly SubscriptionType Premium = new("Premium", true);
    public static readonly SubscriptionType Standard = new("Standard", false);
    public static readonly SubscriptionType Admin = new ("Admin", false);
    
    public string Value { get; private init; }
    public bool HasExpiryDate { get; private set; }

    private SubscriptionType(
        string value,
        bool hasExpiryDate)
    {
        Value = value;
        HasExpiryDate = hasExpiryDate;
    } 

    public static SubscriptionType FromValue(string value) => value switch
    {
        nameof(Premium) => Premium,
        nameof(Standard) => Standard,
        _ => throw new ArgumentException($"Value {value} is out of bound for SubscriptionType.")
    };

    public override string ToString()
        => Value;
}