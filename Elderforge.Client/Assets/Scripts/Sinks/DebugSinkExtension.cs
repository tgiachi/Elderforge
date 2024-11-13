using System;
using Serilog;
using Serilog.Configuration;

public static class MySinkExtensions
{
    public static LoggerConfiguration DebugLog(
              this LoggerSinkConfiguration loggerConfiguration,
              IFormatProvider formatProvider = null)
    {
        return loggerConfiguration.Sink(new DebugSink(formatProvider));
    }
}
