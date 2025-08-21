using System.Threading.Channels;
using discipline.centre.shared.abstractions.Messaging;

namespace discipline.centre.shared.infrastructure.Messaging.Internal.Channels;

internal interface IMessageChannel
{
    ChannelReader<IMessage> Reader { get; }
    ChannelWriter<IMessage> Writer { get; }
}

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IMessage> _messagesChannel = Channel.CreateUnbounded<IMessage>();
    public ChannelReader<IMessage> Reader => _messagesChannel.Reader;
    public ChannelWriter<IMessage> Writer => _messagesChannel.Writer;
}