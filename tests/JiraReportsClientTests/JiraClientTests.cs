using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient;
using JiraReportsClient.Configurations;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Issues.Atlassian.Json;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Http;
using Serilog;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    private static readonly ClientConfiguration Configuration = ClientConfiguration.LoadFromAppSettings();
    private static readonly ILogger Logger = TestLoggerFactory.CreateLoggerAppSettings();
    private static readonly JiraClient Client = new(new JiraHttpClient(Logger, Configuration));
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new BoardRecordKeyConverter(),
            new SprintRecordKeyConverter(),
            new UserSprintRecordKeyConverter()
        }
    };
}