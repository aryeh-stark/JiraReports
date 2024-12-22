using JiraReportsClient.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;

namespace JiraReportsClientTests;

public static class TestLoggerFactory
{
    private static readonly string OutputTemplate = JiraLoggingTemplates.DefaultConsoleTemplate;

    public static ILogger CreateLoggerAppSettings()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return CreateLogger(configuration);
    }

    public static ILogger CreateLogger(IConfiguration configuration)
    {
        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext();

        if (IsTestEnvironment(configuration))
        {
            loggerConfig.WriteTo.Console(outputTemplate: OutputTemplate);
        }

        return loggerConfig.CreateLogger();
    }

    private static bool IsTestEnvironment(IConfiguration configuration)
    {
        return configuration.GetValue<string>("Environment")?.Equals("Test",
            StringComparison.OrdinalIgnoreCase) ?? false;
    }
}