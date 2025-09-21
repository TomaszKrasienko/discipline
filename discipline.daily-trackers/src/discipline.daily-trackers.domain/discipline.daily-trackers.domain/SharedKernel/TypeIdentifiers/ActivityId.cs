namespace discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

public readonly record struct ActivityId(Ulid Value) : ITypeId<ActivityId, Ulid>
{
    public static ActivityId New() => new(Ulid.NewUlid());

    public static ActivityId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(ActivityId)}");
        }

        return new ActivityId(parsedId);
    }

    public override string ToString() => Value.ToString();
}