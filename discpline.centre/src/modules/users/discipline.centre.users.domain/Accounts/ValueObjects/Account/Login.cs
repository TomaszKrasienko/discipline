using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Accounts.Rules.Logins;

namespace discipline.centre.users.domain.Accounts.ValueObjects.Account;

public sealed class Login : ValueObject
{
    private readonly string _value = null!;
    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new LoginCannotBeEmptyRule(value));
            _value = value;
        }
    }

    private Login(string value) => Value = value;

    internal static Login Create(string value) => new(value);
    
    public static implicit operator Login(string value) => Create(value);
    
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}