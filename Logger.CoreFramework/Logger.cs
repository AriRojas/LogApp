using Serilog;
using Serilog.Events;
using System.Configuration;
using System;

namespace Logger.CoreFramework
{
    public static class Logger
    {
        private static readonly ILogger _logger;

        static Logger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File(path: "C:\\Users\\Ariadna\\Documents\\Logs\\LogFile.log")
                .CreateLogger();
        }

        public static void LogPerformance(LogDetail infoLog)
        {
            _logger.Write(LogEventLevel.Information, "{@LogDetail}", infoLog);
        }

        public static void LogUsage(LogDetail infoLog)
        {
            _logger.Write(LogEventLevel.Information, "{@LogDetail}", infoLog);
        }

        public static void LogDiagnostics(LogDetail infoLog)
        {
            var writeDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["EnabelDiagnostics"]);
            if (!writeDiagnostics)
                return;

            _logger.Write(LogEventLevel.Information, "{@LogDetail}", infoLog);
        }

        public static void LogError(LogDetail infoLog)
        {
            _logger.Write(LogEventLevel.Error, "{@LogDetail}", infoLog);
        }
    }
}