using System;
using Elderforge.Network.Client.Services;
using Logger;
using Serilog;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    private void Awake()
    {
        ElderforgeInstanceHolder.Initialize(new LoggerConfiguration().WriteTo.DebugLog());
        Log.Information("Elderforge Client started");
    }
}
