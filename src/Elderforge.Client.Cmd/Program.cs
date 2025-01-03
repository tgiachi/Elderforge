﻿using Elderforge.Network.Client.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Packets.System;
using Elderforge.Network.Types;
using Serilog;


namespace Elderforge.Client.Cmd;

class Program
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();

    private static Task _poolEventTask;

    static void Main(string[] args)
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();


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
                    networkClient.SendMessage(new PongMessage());
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
            () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    networkClient.PoolEvents();
                    Thread.Sleep(30);
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
