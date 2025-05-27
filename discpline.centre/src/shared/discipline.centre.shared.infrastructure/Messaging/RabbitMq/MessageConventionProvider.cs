using System.Reflection;
using discipline.centre.shared.abstractions.Messaging;
using discipline.centre.shared.infrastructure.Messaging.RabbitMq.Abstractions;

namespace discipline.centre.shared.infrastructure.Messaging.RabbitMq;

internal sealed class MessageConventionProvider : IMessageConventionProvider
{
    public (string exchange, string routingKey) Get<TMessage>(TMessage message) where TMessage : class, IMessage
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

        var eventName = typeof(TMessage).Name;
        return $"{messageModuleName}-{PascalToKebabCase(eventName)}";
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