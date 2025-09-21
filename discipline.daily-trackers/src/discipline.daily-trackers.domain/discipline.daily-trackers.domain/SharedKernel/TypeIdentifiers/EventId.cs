namespace discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

public readonly record struct EventId(Ulid Value) : ITypeId<EventId, Ulid>
{
    public static EventId New() => new(Ulid.NewUlid());

    public static EventId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(EventId)}");
        }

        return new EventId(parsedId);
    }
}