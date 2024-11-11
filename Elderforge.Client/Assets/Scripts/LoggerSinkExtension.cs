
using System;
using Serilog;
using Serilog.Configuration;

public static class LoggerSinkExtension
{
    public static LoggerConfiguration UnityDebug(this LoggerSinkConfiguration loggerConfiguration)
    {
        return loggerConfiguration.Sink(new LoggerSink());
    }
}
