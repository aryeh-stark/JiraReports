using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JiraReportsClient.Configurations;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Http.Fluent;
using JiraReportsClient.Logging;
using Serilog;

namespace JiraReportsClient.Http;

public class JiraHttpClient
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

    #endregion

    public JiraHttpClient(ILogger logger, ClientConfiguration config)
    {
        _logger = logger;
        _config = config;
        _endpointBuilder = new JiraEndpointBuilder(BaseUrl, MaxResults);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        _httpClient = new HttpClient();
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_config.UserEmail}:{_config.ApiToken}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

        _logger.Debug($"JiraHttpClient created with {_config}");
    }


    #region Boards

    public async Task<List<Board>> GetBoardsAsync(string projectKey)
    {
        var isLastPage = false;
        var boards = new List<Board>();
        var endpointBuilder = _endpointBuilder.Boards()
            .ForProject(projectKey)
            .WithPagination(-_config.MaxResults, _config.MaxResults);
        do
        {
            var endpoint = endpointBuilder.BuildNextPage();

            _logger
                .WithProjectKey(projectKey)
                .WithEndpoint(endpoint)
                .WithAction()
                .WithCallStep(CallSteps.BeforeCall)
                .WithRange(endpointBuilder)
                .Debug("");

            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                _logger
                    .WithProjectKey(projectKey)
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(endpointBuilder)
                    .Error(string.Empty);
                throw new JiraGetBoardsException(projectKey, endpoint, response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            var boardsResponse = JsonSerializer.Deserialize<BoardsResponse>(content, _jsonOptions);

            isLastPage = (boardsResponse?.IsLast).GetValueOrDefault(true);
            if (boardsResponse?.Values != null)
            {
                boards.AddRange(boardsResponse.Values);
                _logger
                    .WithProjectKey(projectKey)
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(endpointBuilder)
                    .Debug($"Is Last Page: {isLastPage}. Count: {boardsResponse.Values.Count} Content Length: {content.Length}");
            }
            else
            {
                _logger
                    .WithProjectKey(projectKey)
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(endpointBuilder)
                    .Error($"Is Last Page: {isLastPage}. Content Length: {content.Length}");
            }
        } while (!isLastPage);

        return boards;
    }

    #endregion
}