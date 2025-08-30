namespace discipline.hangfire.infrastructure.Messaging.RabbitMq.Configuration;

public class RabbitMqRouteOptions
{
    public List<string> RoutingKey { get; set; }
    public List<string> QueueName { get; set; }
}