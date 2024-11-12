using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Sinks;
using Serilog;
using Serilog.Configuration;

namespace Assets.Extensions
{
    public static class LoggerSinkExtension
    {
        public static LoggerConfiguration UnityDebug(
            this LoggerSinkConfiguration loggerConfiguration,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new UnityDebugSink(formatProvider));
        }
    }
}
