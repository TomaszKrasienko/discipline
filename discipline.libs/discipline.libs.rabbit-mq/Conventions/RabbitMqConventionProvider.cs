using System.Reflection;
using discipline.libs.messaging.Abstractions;
using discipline.libs.rabbit_mq.Conventions.Abstractions;

namespace discipline.libs.rabbit_mq.Conventions;

internal sealed class RabbitMqConventionProvider : IConventionProvider
{
    public (string exchange, string routingKey) GetPublisherRoutes<TMessage>(TMessage message) where TMessage : class, IMessage
    {
        var app = Assembly.GetAssembly(message.GetType())!.GetName().Name!;
        var messageModuleName = string.Empty; 
        
        var indexOfDot = app.LastIndexOf('.');
        
        if (indexOfDot is not -1)
        {
            messageModuleName = app.Substring(0, indexOfDot);
        }
        
        return (messageModuleName,  PascalToKebabCase(message.GetType().Name));
    }

    public string GetQueue<TMessage>() where TMessage : class, IMessage
    {
        var app = Assembly.GetAssembly(typeof(TMessage))!.GetName().Name!;
        var messageModuleName = string.Empty; 
        
        var indexOfDot = app.LastIndexOf('.');
        
        if (indexOfDot is not -1)
        {
            messageModuleName = app.Substring(0, indexOfDot);
            messageModuleName = messageModuleName.Replace('.', '-');
        }

        var messageTypeName = typeof(TMessage).Name;
        return $"{messageModuleName}-{PascalToKebabCase(messageTypeName)}";
    }
    
    private static string PascalToKebabCase(string str)
    {
        return string.Concat(str.SelectMany(ConvertChar));

        IEnumerable<char> ConvertChar(char c, int index)
        {
            if (char.IsUpper(c) && index != 0) yield return '-';
            yield return char.ToLower(c);
        }
    }
}