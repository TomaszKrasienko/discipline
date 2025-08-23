using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public sealed record ActivityRuleId(Ulid Value) : ITypeId<ActivityRuleId, Ulid>
{
    public static ActivityRuleId New() => new(Ulid.NewUlid());
    
    public static ActivityRuleId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("ActivityRuleId.InvalidFormat");
        }

        return new ActivityRuleId(parsedId);
    }

    public override string ToString() => Value.ToString();
}