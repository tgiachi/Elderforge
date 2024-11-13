using Elderforge.Network.Client.Services;
using Serilog;
using UnityEngine;

public class StartupGameScript : MonoBehaviour
{

    void Start()
    {
        ElderforgeInstanceHolder.Initialize(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.DebugLog());

        Log.Information("Initializing Elderforge");

    }


    void Update()
    {

    }
}
