using discipline.activity_scheduler.shared.abstractions.Identifiers;

namespace discipline.activity_scheduler.domain.Accounts;

public sealed class Account
{
    public AccountId AccountId { get; private set; }

    /// <summary>
    /// For EF purposes
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Account()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    
    private Account(AccountId accountId)
    {
        AccountId = accountId;
    }
    
    public static Account Create(AccountId accountId)
        => new (accountId);
}