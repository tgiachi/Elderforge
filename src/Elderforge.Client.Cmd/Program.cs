using Elderforge.Network.Client.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Packets.System;
using Elderforge.Network.Types;


namespace Elderforge.Client.Cmd;

class Program
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();

    private static Task _poolEventTask;

    static void Main(string[] args)
    {
        var messageTypes = new List<MessageTypeObject>()
        {
            new(NetworkMessageType.Ping, typeof(PingMessage)),
            new(NetworkMessageType.Motd, typeof(MotdMessage)),
            new(NetworkMessageType.Version, typeof(VersionMessage))
        };
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            Console.WriteLine("Exiting...");
            _cancellationTokenSource.Cancel();
        };


        var networkClient = new NetworkClient("127.0.0.1", 5000, messageTypes);


        networkClient.SubscribeToMessage<VersionMessage>()
            .Subscribe(
                message =>
                {
                    Console.WriteLine("VersionMessage: " + message.Version);

                }
            );

        networkClient.Connect();



        _poolEventTask = Task.Run(() =>
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                networkClient.PoolEvents();
                Thread.Sleep(30);
            }
        }, _cancellationTokenSource.Token);




        while (true)
        {
            Console.WriteLine("Enter a command:");
            var command = Console.ReadLine();

            if (command == "exit")
            {
                break;
            }
        }
    }
}
