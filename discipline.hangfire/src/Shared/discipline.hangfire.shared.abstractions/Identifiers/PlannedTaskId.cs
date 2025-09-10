namespace discipline.hangfire.shared.abstractions.Identifiers;

public readonly record struct PlannedTaskId(Ulid Value)
{
    public static PlannedTaskId New()
        => new (Ulid.NewUlid());

    public override string ToString()
        => Value.ToString();

    public static PlannedTaskId Parse(string stringTypedId)
    {
        if (!Ulid.TryParse(stringTypedId, out var parsedId))
        {
            throw new ArgumentException($"Can not parse stronglyTypedId of type: {nameof(PlannedTaskId)}");
        }

        return new PlannedTaskId(parsedId);
    }
}