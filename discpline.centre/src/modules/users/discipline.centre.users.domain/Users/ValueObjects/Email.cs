using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.users.domain.Users.Rules.Users;

namespace discipline.centre.users.domain.Users.ValueObjects;

public sealed class Email : ValueObject
{
    private readonly string _value = null!;
    public string Value
    {
        get => _value;
        private init
        {
            CheckRule(new EmailCanNotBeEmptyRule(value));
            CheckRule(new EmailValidFormatRule(value));
            _value = value;
        }
    }
    
    private Email(string value) => Value = value;

    public static Email Create(string value) => new(value);

    public static implicit operator Email(string email) => Create(email);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}