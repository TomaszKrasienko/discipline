using discipline.hangfire.shared.abstractions.Identifiers;

namespace discipline.hangfire.account_modification.Models;

internal sealed class Account
{
    public AccountId AccountId { get; private set; }

    private Account()
    {
    }
    
    private Account(AccountId accountId)
    {
        AccountId = accountId;
    }
    
    public static Account Create(AccountId accountId)
        => new (accountId);
}