using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;

namespace Assets.Scripts.Sinks
{
    public class UnityDebugSink : ILogEventSink
    {

        private readonly IFormatProvider _formatProvider;


        public UnityDebugSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);

            if (logEvent.Exception != null)
            {
                message += Environment.NewLine + logEvent.Exception;
            }

            switch (logEvent.Level)
            {
                case LogEventLevel.Verbose:
                case LogEventLevel.Debug:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogEventLevel.Information:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogEventLevel.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogEventLevel.Error:
                case LogEventLevel.Fatal:
                    UnityEngine.Debug.LogError(message);
                    break;
                default:
                    UnityEngine.Debug.Log(message);
                    break;
            }
        }
    }
}
