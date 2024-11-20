using System;
using Serilog;
using Serilog.Configuration;

namespace Logger
{
    public static class DebuggerLoggerExtension
    {
        public static LoggerConfiguration DebugLog(
            this LoggerSinkConfiguration loggerConfiguration,
            IFormatProvider formatProvider = null
        )
        {
            return loggerConfiguration.Sink(new DebugSink(formatProvider));
        }
    }
}
