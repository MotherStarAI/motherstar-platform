using Microsoft.Extensions.Configuration;
using Serilog;

namespace MotherStar.Platform.Bootstrapper
{
    public static class SerilogBootstrapper
    {
        public static LoggerConfiguration BuildLoggerConfig(IConfiguration configuration, string environmentName)
        {
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var logConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                // Just adding Enrichers in code to keep config cleaner, we will always want them
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("ApplicationName", "LighthouseApi");

            return logConfig;
        }
    }
}
