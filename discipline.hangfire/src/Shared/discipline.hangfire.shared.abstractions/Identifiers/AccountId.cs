namespace discipline.hangfire.shared.abstractions.Identifiers;

public sealed record AccountId(Ulid Value)
{
    public static AccountId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();

    public static AccountId Empty()
        => new AccountId(Ulid.Empty);

    public static AccountId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(AccountId)}");
        }

        return new AccountId(parsedId);
    }
}