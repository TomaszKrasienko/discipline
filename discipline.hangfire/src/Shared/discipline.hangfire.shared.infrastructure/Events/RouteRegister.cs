using discipline.hangfire.infrastructure.Events.Abstractions;
using discipline.hangfire.shared.abstractions.Events;

namespace discipline.hangfire.infrastructure.Events;

internal sealed class RouteRegister : IRouteRegister
{
    public string GetChannel<T>() where T : class, IEvent
        => typeof(T).Name;
}