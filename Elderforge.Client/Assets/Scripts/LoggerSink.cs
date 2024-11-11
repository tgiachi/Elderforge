using System.Collections;
using System.Collections.Generic;
using Data;
using Serilog.Core;
using Serilog.Events;
using UnityEngine;

public class LoggerSink : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        InstanceHolder.EventBusService.Publish(new LoggerEvent(logEvent.RenderMessage(), logEvent.Level.ToString()));
        Debug.Log(logEvent.RenderMessage());
    }
}
