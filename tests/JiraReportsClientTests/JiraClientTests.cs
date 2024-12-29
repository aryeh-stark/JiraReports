using JiraReportsClient;
using JiraReportsClient.Configurations;
using JiraReportsClient.Http;
using Serilog;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    private static readonly ClientConfiguration Configuration = ClientConfiguration.LoadFromAppSettings();
    private static readonly ILogger Logger = TestLoggerFactory.CreateLoggerAppSettings();
    private static readonly JiraClient Client = new(new JiraHttpClient(Logger, Configuration));
}