using Assets.Extensions;
using Elderforge.Network.Client.Services;
using Serilog;
using UnityEngine;

namespace Assets.Scripts
{
    public class Init : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ElderforgeInstanceHolder.Initialize(new LoggerConfiguration().WriteTo.UnityDebug());


        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}
