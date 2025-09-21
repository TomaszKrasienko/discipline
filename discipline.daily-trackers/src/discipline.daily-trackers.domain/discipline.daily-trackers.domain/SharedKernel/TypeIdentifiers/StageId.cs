namespace discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

public readonly record struct StageId(Ulid Value) : ITypeId<StageId, Ulid>
{
    public static StageId New() => new(Ulid.NewUlid());
    
    public static StageId Parse(string stringTypedId)
    { 
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException("StageId.InvalidFormat");
        }

        return new StageId(parsedId);
    }

    public override string ToString() => Value.ToString();
}