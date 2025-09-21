namespace discipline.daily_trackers.domain.SharedKernel.TypeIdentifiers;

public readonly record struct DailyTrackerId(Ulid Value) : ITypeId<DailyTrackerId, Ulid>
{
    public static DailyTrackerId New() => new(Ulid.NewUlid());

    public static DailyTrackerId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(DailyTrackerId)}");
        }

        return new DailyTrackerId(parsedId);
    }

    public override string ToString() => Value.ToString();
}