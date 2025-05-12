using Microsoft.Extensions.Options;

namespace discipline.centre.shared.infrastructure.Events.Brokers.RabbitMq.Configuration;

internal sealed class RabbitMqOptionsValidator : IValidateOptions<RabbitMqOptions>
{
    public ValidateOptionsResult Validate(string? name, RabbitMqOptions options)
    {
        if (string.IsNullOrWhiteSpace(options?.HostName))
        {
            return ValidateOptionsResult.Fail("RabbitMQ HostName can not be null or empty");
        }
        
        if (string.IsNullOrWhiteSpace(options?.VirtualHost))
        {
            return ValidateOptionsResult.Fail("RabbitMQ VirtualHost can not be null or empty");
        }
        
        if (string.IsNullOrWhiteSpace(options?.Password))
        {
            return ValidateOptionsResult.Fail("RabbitMQ Password can not be null or empty");
        }
        
        if (string.IsNullOrWhiteSpace(options?.Username))
        {
            return ValidateOptionsResult.Fail("RabbitMQ Username can not be null or empty");
        }
        
        return ValidateOptionsResult.Success;
    }
}