using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Interfaces.Listeners;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Interfaces.Services;
using Elderforge.Network.Types;
using Serilog;
using ListenerResult =
    System.Func<string, Elderforge.Network.Interfaces.Messages.INetworkMessage, System.Threading.Tasks.ValueTask<
        System.Collections.Generic.IEnumerable<Elderforge.Network.Data.Internal.SessionNetworkMessage>>>;

namespace Elderforge.Network.Services;

public class MessageDispatcherService : IMessageDispatcherService
{
    private readonly ILogger _logger = Log.ForContext<MessageDispatcherService>();
    private readonly IMessageTypesService _messageTypesService;
    private readonly INetworkMessageFactory _networkMessageFactory;


    private readonly IMessageChannelService _messageChannelService;

    private readonly Task _dispatchIncomingMessagesTask;

    private readonly CancellationTokenSource _incomingMessagesCancellationTokenSource = new();

    private readonly ConcurrentDictionary<NetworkMessageType, List<ListenerResult>> _handlers = new();

    public MessageDispatcherService(
        IMessageTypesService messageTypesService, INetworkMessageFactory networkMessageFactory,
        IMessageChannelService messageChannelService
    )
    {
        _messageTypesService = messageTypesService;

        _networkMessageFactory = networkMessageFactory;
        _messageChannelService = messageChannelService;

        _dispatchIncomingMessagesTask = DispatchIncomingMessages();
    }

    private async Task DispatchIncomingMessages()
    {
        while (!_incomingMessagesCancellationTokenSource.Token.IsCancellationRequested)
        {
            await foreach (var message in _messageChannelService.IncomingReaderChannel.ReadAllAsync(
                               _incomingMessagesCancellationTokenSource.Token
                           ))
            {
                var parsedMessage = await _networkMessageFactory.ParseAsync(message.Packet);

                DispatchMessage(message.SessionId, parsedMessage);
            }
        }
    }

    public void RegisterMessageListener<TMessage>(INetworkMessageListener<TMessage> listener)
        where TMessage : class, INetworkMessage
    {
        RegisterMessageListener<TMessage>(
            async (sessionId, message) => await listener.OnMessageReceivedAsync(sessionId, message)
        );
    }


    public void RegisterMessageListener<TMessage>(
        Func<string, TMessage, ValueTask<IEnumerable<SessionNetworkMessage>>> listener
    ) where TMessage : class, INetworkMessage
    {
        var messageType = _messageTypesService.GetMessageType(typeof(TMessage));

        if (!_handlers.TryGetValue(messageType, out var handlers))
        {
            handlers = new List<Func<string, INetworkMessage, ValueTask<IEnumerable<SessionNetworkMessage>>>>();
            _handlers.TryAdd(messageType, handlers);
        }

        handlers.Add(
            async (sessionId, message) =>
            {
                if (message is TMessage typedMessage)
                {
                    return await listener.Invoke(sessionId, typedMessage);
                }

                return new List<SessionNetworkMessage>();
            }
        );
    }

    public async void DispatchMessage<TMessage>(string sessionId, TMessage message) where TMessage : class, INetworkMessage
    {
        var messageType = _messageTypesService.GetMessageType(typeof(TMessage));

        if (!_handlers.TryGetValue(messageType, out var handlers))
        {
            _logger.Warning("No handlers registered for message type {messageType}", messageType);
            return;
        }

        var messageToSend = new List<SessionNetworkMessage>();
        foreach (var handler in handlers)
        {
            var result = await handler.Invoke(sessionId, message);
            messageToSend.AddRange(result);
        }

        foreach (var sessionNetworkMessage in messageToSend)
        {
            await _messageChannelService.OutgoingWriterChannel.WriteAsync(sessionNetworkMessage);
        }
    }

    public void Dispose()
    {
        _dispatchIncomingMessagesTask.Wait();
        _incomingMessagesCancellationTokenSource.Cancel();
        _incomingMessagesCancellationTokenSource.Dispose();
    }
}
