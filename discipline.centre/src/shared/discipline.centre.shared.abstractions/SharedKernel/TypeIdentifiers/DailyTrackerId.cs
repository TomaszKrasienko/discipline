using discipline.centre.shared.abstractions.Exceptions;

namespace discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;

public record DailyTrackerId(Ulid Value) : ITypeId<DailyTrackerId, Ulid>
{
    public static DailyTrackerId New() => new(Ulid.NewUlid());

    public static DailyTrackerId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new InvalidArgumentException("DailyTrackerId.InvalidFormat");
        }

        return new DailyTrackerId(parsedId);
    }

    public override string ToString() => Value.ToString();
}