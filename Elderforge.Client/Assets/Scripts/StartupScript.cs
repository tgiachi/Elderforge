using System.Collections;
using System.Collections.Generic;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Types;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    private NetworkClient _networkClient;

    public StartupScript()
    {
        var messages = new List<MessageTypeObject>();

        messages.Add(new MessageTypeObject(NetworkMessageType.Ping, typeof(PingMessage)));
        messages.Add(new MessageTypeObject(NetworkMessageType.Motd, typeof(MotdMessage)));
        _networkClient = new NetworkClient("127.0.0.1", 5000, messages);
    }


    // Start is called before the first frame update
    void Start()
    {
        _networkClient.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        _networkClient.PoolEvents();
    }
}
