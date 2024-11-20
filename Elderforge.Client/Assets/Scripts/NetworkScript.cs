using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Interfaces.Messages;
using Serilog;
using UnityEngine;

namespace Elderforge.Client.Assets.Scripts
{
    public class NetworkScript : MonoBehaviour
    {
        private readonly List<Task> _tasks = new();


        [Header("Internal")] [SerializeField] public int outputTaskCount = 1;

        [SerializeField] public int refreshTickRate = 20;

        [Header("Server")] [SerializeField] public string serverIp = "127.0.0.1";
        [SerializeField] public int serverPort = 5000;

        private bool isRunning = true;

        private readonly ConcurrentQueue<INetworkMessage> _messageQueue = new();


        private void Start()
        {
            for (var i = 0; i < outputTaskCount; i++)
            {
                _tasks.Add(CreateOutputTask(i));
            }

            _tasks.Add(PoolEventTask());
        }

        private void OnDestroy()
        {
            isRunning = false;
        }

        private async Task CreateOutputTask(int id)
        {
            Log.Information("Starting output task {id}", id);
            while (isRunning)
            {
                if (_messageQueue.TryDequeue(out var message))
                {
                    await ElderforgeInstanceHolder.NetworkClient.SendMessageAsync(message);
                }

                await Task.Delay(refreshTickRate);
            }
        }

        private async Task PoolEventTask()
        {
            Log.Information("Starting pool event task");
            while (isRunning)
            {
                ElderforgeInstanceHolder.NetworkClient.PoolEvents();
                await Task.Delay(refreshTickRate);
            }
        }

        public void SendMessage(INetworkMessage message)
        {
            _messageQueue.Enqueue(message);
        }
    }
}
