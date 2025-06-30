using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Accounts.Rules.Passwords;

namespace discipline.centre.users.domain.Accounts.ValueObjects.Account;

public sealed class Password : ValueObject
{
    public string Value { get; }

    private Password(string value) => Value = value;

    internal static Password Create(string password, string hashedPassword)
    {
        CheckRule(new PasswordMustBeStrongRule(password));
        return new Password(hashedPassword);
    }
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}