using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.users.domain.Accounts.Enums;

public readonly record struct AccountState
{
    public static AccountState New => new ("New");
    public static AccountState Active => new ("Active");
    
    public string Value { get; }

    private AccountState(string value)
        => Value = value;

    public static AccountState FromValue(string value) => value switch
    {
        nameof(New) => New,
        nameof(Active) => Active,
        _ => throw new InvalidArgumentException("AccountState.ArgumentOutOfBound")
    };
}