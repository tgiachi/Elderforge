﻿using System.Numerics;
using Elderforge.Network.Client.Interfaces;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Interfaces.Messages;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Packets.Player;
using Elderforge.Network.Packets.System;
using Elderforge.Network.Serialization.Numerics;
using Serilog;
using Serilog.Core;


namespace Elderforge.Client.Cmd;

class Program
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();

    private static Task _poolEventTask;

    private static Task _moveTask;

    private static Vector3 _myPosition = new(0, 0, 0);

    private static Vector3 _myRotation = new(0, 0, 0);


    private static void SendMessage<TMessage>(INetworkClient networkClient, TMessage message)
        where TMessage : class, INetworkMessage
    {
        if (networkClient != null && networkClient.IsConnected)
        {
            Log.Information(">> Sending {messageType}: {Message}", typeof(TMessage).Name, message);
            networkClient.SendMessageAsync<TMessage>(message);
        }
    }

    static async Task Main(string[] args)
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = logger;


        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            logger.Information("Exiting...");
            _cancellationTokenSource.Cancel();
            Environment.Exit(0);
        };


        var networkClient = new NetworkClient(ElderforgeInstanceHolder.MessageTypes);

        networkClient.SubscribeToMessage<PingMessage>()
            .Subscribe(
                pingMessage =>
                {
                    logger.Information("Received ping message, sending pong message");
                    SendMessage(networkClient, new PongMessage());
                }
            );

        networkClient.SubscribeToMessage<MotdMessage>()
            .Subscribe(
                motdMessage =>
                {
                    foreach (var motdLine in motdMessage.Lines)
                    {
                        logger.Information("MOTD: {motdLine}", motdLine);
                    }
                }
            );

        networkClient.SubscribeToMessage<VersionMessage>()
            .Subscribe(
                versionMessage => { logger.Information("Server version: {version}", versionMessage.Version); }
            );


        networkClient.MessageReceived += (messageType, message) =>
        {
            logger.Information("Received message of type {messageType}: {Message}", messageType, message);
        };

        networkClient.Connect("127.0.0.1", 5000);


        _poolEventTask = Task.Run(
            async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    networkClient.PoolEvents();
                    await Task.Delay(30);
                }
            },
            _cancellationTokenSource.Token
        );

        _moveTask = Task.Run(
            async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    _myPosition += new Vector3(0.1f, 0, 0);
                    _myRotation += new Vector3(0, 0.1f, 0);

                    var movePlayerMessage = new PlayerMoveRequestMessage(
                        new SerializableVector3(_myPosition),
                        new SerializableVector3(_myRotation)
                    );

                    SendMessage(networkClient, movePlayerMessage);

                    await Task.Delay(1000);
                }
            },
            _cancellationTokenSource.Token
        );


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
