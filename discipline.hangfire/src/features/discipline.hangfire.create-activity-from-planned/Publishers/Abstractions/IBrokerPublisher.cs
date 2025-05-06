namespace discipline.hangfire.create_activity_from_planned.Publishers.Abstractions;

internal interface IBrokerPublisher
{
    Task SendAsync<T>(T message, CancellationToken cancellationToken) where T : class;
}