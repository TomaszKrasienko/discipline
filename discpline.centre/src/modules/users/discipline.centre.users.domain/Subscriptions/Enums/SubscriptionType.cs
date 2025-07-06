namespace discipline.centre.users.domain.Subscriptions.Enums;

public sealed record SubscriptionType
{
    public static readonly SubscriptionType Premium = new("Premium", true, true);
    public static readonly SubscriptionType Standard = new("Standard", false, false);
    public static readonly SubscriptionType Admin = new ("Admin", false, false);
    
    public string Value { get; }
    public bool HasExpiryDate { get; }
    public bool HasPayment { get; }

    private SubscriptionType(
        string value,
        bool hasExpiryDate,
        bool hasPayment)
    {
        Value = value;
        HasExpiryDate = hasExpiryDate;
        HasPayment = hasPayment;
    } 

    public static SubscriptionType FromValue(string value) => value switch
    {
        nameof(Premium) => Premium,
        nameof(Standard) => Standard,
        nameof(Admin) => Admin,
        _ => throw new ArgumentException($"Value {value} is out of bound for SubscriptionType.")
    };

    public override string ToString()
        => Value;
}