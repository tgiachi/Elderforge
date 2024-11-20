using System;
using Serilog.Core;
using Serilog.Events;
using UnityEngine;

public class DebugSink : ILogEventSink
{

    public DebugSink(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }
    private readonly IFormatProvider _formatProvider;

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        switch (logEvent.Level)
        {
            case LogEventLevel.Debug:
                Debug.Log(message);
                break;
            case LogEventLevel.Information:
                Debug.Log(message);
                break;
            case LogEventLevel.Warning:
                Debug.LogWarning(message);
                break;
            case LogEventLevel.Error:
                Debug.LogError(message);
                break;
            case LogEventLevel.Fatal:
                Debug.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
