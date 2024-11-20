using System;
using Elderforge.Client.Assets.Scripts;
using Elderforge.Network.Client.Services;
using Logger;
using Serilog;
using UnityEngine;

public class StartupScript : MonoBehaviour
{

    private NetworkScript _networkScript;

    private void Awake()
    {
        ElderforgeInstanceHolder.Initialize(new LoggerConfiguration().WriteTo.DebugLog());
        Log.Information("Elderforge Client started");
        _networkScript = GetComponent<NetworkScript>();

        if (_networkScript == null)
        {
            Log.Error("NetworkScript not found");
            return;
        }
    }
}
