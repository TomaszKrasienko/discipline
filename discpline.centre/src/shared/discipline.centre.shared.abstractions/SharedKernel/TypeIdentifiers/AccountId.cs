namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public readonly record struct AccountId(Ulid Value) : ITypeId<AccountId, Ulid>
{
    public static AccountId New() => new(Ulid.NewUlid());

    public static AccountId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsed))
        {
            throw new ArgumentException($"Invalid account Id value: {parsed}");
        }
        
        return new AccountId(parsed);   
    }
}