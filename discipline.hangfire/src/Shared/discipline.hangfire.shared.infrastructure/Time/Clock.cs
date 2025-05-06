using discipline.hangfire.shared.abstractions.Time;

namespace discipline.hangfire.infrastructure.Time;

internal sealed class Clock : IClock
{
    public DateTime Now()
        => DateTime.UtcNow;
}