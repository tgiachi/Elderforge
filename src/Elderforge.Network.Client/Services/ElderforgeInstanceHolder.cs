using System;
using System.Collections.Generic;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Services;
using Elderforge.Network.Client.Interfaces;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Packets.System;
using Elderforge.Network.Types;
using Serilog;

namespace Elderforge.Network.Client.Services;

public class ElderforgeInstanceHolder
{
    private static ElderforgeInstanceHolder? _instance;

    public static ElderforgeInstanceHolder Instance => _instance;

    public static INetworkClient NetworkClient { get; } = new NetworkClient(MessageTypes);

    public static List<MessageTypeObject> MessageTypes =>
    [
        new(NetworkMessageType.Ping, typeof(PingMessage)),
        new(NetworkMessageType.Motd, typeof(MotdMessage)),
        new(NetworkMessageType.MotdRequest, typeof(MotdRequestMessage)),

        new(NetworkMessageType.Version, typeof(VersionMessage)),
        new(NetworkMessageType.Pong, typeof(PongMessage)),
        new(NetworkMessageType.ServerReady, typeof(ServerReadyMessage)),
    ];

    public IEventBusService EventBusService { get; } = new EventBusService();


    public static void Initialize(LoggerConfiguration configuration)
    {
        _instance = new ElderforgeInstanceHolder(configuration);


        NetworkClient.MessageReceived += (messageType, message) =>
        {
            Log.Logger.Debug("Received message of type {MessageType}", messageType);
        };
        NetworkClient.SubscribeToMessage<PingMessage>()
            .Subscribe(
                message =>
                {
                    Log.Logger.Debug("Received ping message, sending pong message");
                    NetworkClient.SendMessage(new PongMessage());
                }
            );
    }


    public void SendEvent<TEvent>(TEvent eventItem) where TEvent : class
    {
        EventBusService.Publish(eventItem);
    }

    public void SubscribeEvent<TEvent>(Action<TEvent> handler) where TEvent : class
    {
        EventBusService.Subscribe(handler);
    }

    public ElderforgeInstanceHolder(LoggerConfiguration configuration)
    {
        Log.Logger = configuration.CreateLogger();
    }
}
