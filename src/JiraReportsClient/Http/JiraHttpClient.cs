using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient.Configurations;
using Serilog;

namespace JiraReportsClient;

public class JiraHttpClient
{
    #region Fields

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ClientConfiguration _config;
    private readonly ILogger _logger;

    #endregion

    public JiraHttpClient(ILogger logger, ClientConfiguration config)
    {
        _logger = logger;
        _config = config;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        _httpClient = new HttpClient();
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_config.UserEmail}:{_config.ApiToken}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        _logger.Debug($"JiraHttpClient created with {_config}");
    }
}