using discipline.hangfire.shared.abstractions.Events;

namespace discipline.hangfire.infrastructure.Events.Abstractions;

public interface IRouteRegister
{
    string GetChannel<T>() where T : class, IEvent;
}