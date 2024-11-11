using System.Collections;
using System.Collections.Generic;
using Elderforge.Network.Client.Services;
using Elderforge.Network.Data.Internal;
using Elderforge.Network.Packets;
using Elderforge.Network.Packets.Motd;
using Elderforge.Network.Types;
using Serilog;
using TMPro;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    private NetworkClient _networkClient;

    public StartupScript()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.UnityDebug()
            .CreateLogger();
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
