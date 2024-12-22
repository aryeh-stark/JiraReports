using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JiraReportsClient.Configurations;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Http.EndpointFluentBuilder;
using JiraReportsClient.Logging;
using Serilog;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
    #region Boards

    public async Task<List<Board>> GetBoardsForProjectAsync(string projectKey)
    {
        var isLastPage = false;
        var boards = new List<Board>();
        var endpointBuilder = _endpointBuilder.Boards()
            .ForProject(projectKey)
            .WithPagination();
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
                var message = await response.Content.ReadAsStringAsync();
                _logger
                    .WithProjectKey(projectKey)
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(endpointBuilder)
                    .WithErrorResponse(message)
                    .Error(string.Empty);
                throw new JiraGetBoardsForProjectException(projectKey, endpoint, response.StatusCode);
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
    
    public async Task<Board?> GetBoardByIdAsync(int boardId)
    {
        var endpoint =_endpointBuilder.Boards()
            .ById(boardId)
            .Build();

        _logger
            .WithBoardId(boardId)
            .WithEndpoint(endpoint)
            .WithAction()
            .WithCallStep(CallSteps.BeforeCall)
            .Debug("");

        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            _logger
                .WithBoardId(boardId)
                .WithEndpoint(endpoint)
                .WithAction()
                .WithStatusCode(response.StatusCode)
                .WithCallStep(CallSteps.AfterCall)
                .WithErrorResponse(message)
                .Error("");
            throw new JiraGetBoardByIdException(boardId, endpoint, response.StatusCode);
        }

        var content = await response.Content.ReadAsStringAsync();
        var board = JsonSerializer.Deserialize<Board>(content, _jsonOptions);

        if (board == null)
        {
            _logger
                .WithBoardId(boardId)
                .WithEndpoint(endpoint)
                .WithAction()
                .WithStatusCode(response.StatusCode)
                .WithCallStep(CallSteps.AfterCall)
                .Error("");
            throw new JiraGetBoardByIdDeserializationException(boardId, endpoint, response.StatusCode);
        }
        
        _logger
            .WithBoardId(boardId)
            .WithEndpoint(endpoint)
            .WithAction()
            .WithStatusCode(response.StatusCode)
            .WithCallStep(CallSteps.AfterDeserialization)
            .Debug($"Content Length: {content.Length}");
        return board;
    }
    
    public async Task<List<Board>> GetBoardsAsync()
    {
        var boardsCollection = new List<Board>();
        var endpointBuilder = _endpointBuilder.Boards()
            .WithPagination();
        var isLastPage = false;
        do
        {
            var endpoint = endpointBuilder.BuildNextPage();

            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithCallStep(CallSteps.BeforeCall)
                .WithRange(endpointBuilder)
                .Debug("");

            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                _logger
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(endpointBuilder)
                    .WithErrorResponse(message)
                    .Error(string.Empty);
                throw new JiraGetBoardsException(endpoint, response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            var boardsResponse = JsonSerializer.Deserialize<BoardsResponse>(content, _jsonOptions);

            isLastPage = (boardsResponse?.IsLast).GetValueOrDefault(true);
            if (boardsResponse?.Values != null)
            {
                boardsCollection.AddRange(boardsResponse.Values);
                _logger
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
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(endpointBuilder)
                    .Error($"Is Last Page: {isLastPage}. Content Length: {content.Length}");
            }
        } while (!isLastPage);

        _logger.Debug($"Total boards: {boardsCollection.Count}");
        return boardsCollection;
    }
    
    public async Task<(bool, Board? board)> IsBoardScrum(int boardId)
    {
        var board = await GetBoardByIdAsync(boardId);
        return (board is { Type: BoardTypes.Scrum }, board);
    }  

    #endregion
}