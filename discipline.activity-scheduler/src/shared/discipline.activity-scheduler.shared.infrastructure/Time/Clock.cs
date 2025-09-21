using discipline.activity_scheduler.shared.abstractions.Time;

namespace discipline.activity_scheduler.shared.infrastructure.Time;

internal sealed class Clock : IClock
{
    public DateTime Now()
        => DateTime.UtcNow;
}