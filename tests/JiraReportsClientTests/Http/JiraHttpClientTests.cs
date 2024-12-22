using JiraReportsClient.Configurations;
using JiraReportsClient.Http;
using Serilog;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{
    private static readonly ClientConfiguration Configuration = ClientConfiguration.LoadFromAppSettings();
    private static readonly ILogger Logger = TestLoggerFactory.CreateLoggerAppSettings();
    private static readonly JiraHttpClient Client = new(Logger, Configuration);
}