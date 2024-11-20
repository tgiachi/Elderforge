using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elderforge.Core.Interfaces.Services;
using Elderforge.Core.Services;
using Elderforge.Network.Client.Interfaces;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Packets.Chat;
using Elderforge.Network.Packets.GameObjects;
using Elderforge.Network.Packets.GameObjects.Lights;
using Elderforge.Network.Packets.Login;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Packets.Player;
using Elderforge.Network.Packets.System;
using Elderforge.Network.Packets.TimeAndWeather;
using Elderforge.Network.Packets.World;
using Elderforge.Network.Types;
using Serilog;

namespace Elderforge.Network.Client.Services;

public class ElderforgeInstanceHolder
{
    private static ElderforgeInstanceHolder? _instance;

    public static ElderforgeInstanceHolder Instance => _instance;


    private readonly CancellationTokenRegistration _cancellationTokenRegistration = new();

    public static INetworkClient NetworkClient { get; private set; }

    public static List<MessageTypeObject> MessageTypes =>
    [
        new(NetworkMessageType.Ping, typeof(PingMessage)),
        new(NetworkMessageType.Pong, typeof(PongMessage)),

        new(NetworkMessageType.Motd, typeof(MotdMessage)),
        new(NetworkMessageType.MotdRequest, typeof(MotdRequestMessage)),

        new(NetworkMessageType.Version, typeof(VersionMessage)),

        new(NetworkMessageType.ServerReady, typeof(ServerReadyMessage)),
        new(NetworkMessageType.ServerShutdown, typeof(ServerShutdownMessage)),
        new(NetworkMessageType.Chat, typeof(ChatMessage)),

        new(NetworkMessageType.WorldChunkRequest, typeof(WorldChunkRequestMessage)),
        new(NetworkMessageType.WorldChunkResponse, typeof(WorldChunkResponseMessage)),

        new(NetworkMessageType.GameObjectDestroyResponse, typeof(GameObjectDestroyMessage)),
        new(NetworkMessageType.GameObjectActionRequest, typeof(GameObjectActionRequestMessage)),

        new(NetworkMessageType.LightUpdateResponse, typeof(LightGoUpdateResponseMessage)),

        new(NetworkMessageType.LoginRequest, typeof(LoginRequestMessage)),
        new(NetworkMessageType.LoginResponse, typeof(LoginResponseMessage)),

        new(NetworkMessageType.TimeChangedResponse, typeof(TimeChangedMessage)),
        new(NetworkMessageType.DayPhaseChangedResponse, typeof(DayPhaseChangedMessage)),

        new(NetworkMessageType.PlayerMoveRequest, typeof(PlayerMoveRequestMessage)),
        new(NetworkMessageType.PlayerUpdateResponse, typeof(PlayerUpdateResponseMessage)),
        new(NetworkMessageType.PlayerDisconnectedResponse, typeof(PlayerDisconnectedMessage))
    ];

    public IEventBusService EventBusService { get; } = new EventBusService();


    public static void Initialize(LoggerConfiguration configuration)
    {
        _instance = new ElderforgeInstanceHolder(configuration);

        NetworkClient = new NetworkClient(MessageTypes);


        NetworkClient.MessageReceived += (messageType, message) =>
        {
            Log.Debug("Received message of type {MessageType}", messageType);
        };
        // NetworkClient.SubscribeToMessage<PingMessage>()
        //     .Subscribe(
        //         message =>
        //         {
        //             Log.Logger.Debug("Received ping message, sending pong message");
        //             NetworkClient.SendMessageAsync(new PongMessage());
        //         }
        //     );
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

    public void PoolEventTask()
    {
        Task.Run(
            async () =>
            {
                while (!_cancellationTokenRegistration.Token.IsCancellationRequested)
                {
                    await Task.Delay(15);
                    NetworkClient.PoolEvents();
                }
            }
        );
    }

    public void StopPooling()
    {
        _cancellationTokenRegistration.Dispose();
    }
}
