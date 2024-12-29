using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient.Configurations;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Issues.Atlassian.Json;
using JiraReportsClient.Http.EndpointFluentBuilder;
using JiraReportsClient.Logging;
using JiraReportsClient.Utils.Json.Converters;
using Serilog;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
    #region Fields

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ClientConfiguration _config;
    private readonly ILogger _logger;
    private readonly JiraEndpointBuilder _endpointBuilder;

    #endregion

    #region Properties

    private string BaseUrl => _config.BaseUrl;
    private int MaxResults => _config.MaxResults;

    private int ChunkSize => _config.JqlChunkSize;

    #endregion

    public JiraHttpClient(ILogger logger, ClientConfiguration config)
    {
        _logger = logger;
        _config = config;
        _endpointBuilder = new JiraEndpointBuilder(BaseUrl, MaxResults);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
        
        // Add the converter factory
        _jsonOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        _jsonOptions.Converters.Add(new IssueFieldsConverterFactory(config.CustomFieldMapping.Mapping));

        _httpClient = new HttpClient();
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_config.UserEmail}:{_config.ApiToken}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        _logger.Debug($"JiraHttpClient created with {_config}");
    }
   
}