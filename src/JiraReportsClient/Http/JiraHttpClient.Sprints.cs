using System.Text.Json;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Boards.Atlassian;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Issues.Atlassian;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Entities.Sprints.Atlassian;
using JiraReportsClient.Http.EndpointFluentBuilder;
using JiraReportsClient.Logging;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
    public async Task<List<JiraSprint>> GetSprintsForBoardIdAsync(int boardId,
        bool includeFutureSprints = false,
        bool includeActiveSprints = false,
        bool includeClosedSprints = false)
    {
        var (isScrumBoard, board) = await IsBoardScrum(boardId);
        if (isScrumBoard == false)
            throw new JiraNotScrumBoardException(boardId);

        var sprintsEndpointBuilder = _endpointBuilder.Sprints()
            .ForBoard(boardId)
            .IncludeStates(future: includeFutureSprints, active: includeActiveSprints, closed: includeClosedSprints)
            .WithPagination();
        var sprintList = new List<JiraSprint>();
        var lastPage = false;
        do
        {
            var endpoint = sprintsEndpointBuilder.BuildNextPage();
            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithBoardId(boardId)
                .WithCallStep(CallSteps.BeforeCall)
                .WithRange(sprintsEndpointBuilder)
                .WithSprintStates(future: includeFutureSprints, active: includeActiveSprints,
                    closed: includeClosedSprints)
                .Debug("");
            try
            {
                var response = await _httpClient.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger
                        .WithEndpoint(endpoint)
                        .WithAction()
                        .WithBoardId(boardId)
                        .WithStatusCode(response.StatusCode)
                        .WithCallStep(CallSteps.AfterCall)
                        .WithRange(sprintsEndpointBuilder)
                        .WithSprintStates(future: includeFutureSprints, active: includeActiveSprints,
                            closed: includeClosedSprints)
                        .WithErrorResponse(message)
                        .Error("");
                    throw new JiraGetSprintsForBoardException(message, boardId, endpoint, response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();
                var sprintsResponse = JsonSerializer.Deserialize<JiraSprintResponse>(content, _jsonOptions);

                if (sprintsResponse == null || !sprintsResponse.HasValues())
                    throw new JiraGetSprintsForBoardDeserializationException(boardId, endpoint, response.StatusCode);

                sprintList.AddRange(sprintsResponse.Values.Select(s =>
                {
                    s.JiraBoard = board!;
                    return s;
                }));
                lastPage = sprintsResponse.IsLast;

                _logger
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithBoardId(boardId)
                    .WithStatusCode(response.StatusCode)
                    .WithCallStep(CallSteps.AfterCall)
                    .WithRange(sprintsEndpointBuilder)
                    .WithSprintStates(future: includeFutureSprints, active: includeActiveSprints,
                        closed: includeClosedSprints)
                    .Debug(
                        $"Is Last Page: {sprintsResponse.IsLast}. Count: {sprintsResponse.Count} Content Length: {content.Length}");
            }
            catch (Exception ex)
            {
                _logger
                    .WithEndpoint(endpoint)
                    .WithAction()
                    .WithBoardId(boardId)
                    .WithException(ex)
                    .WithCallStep(CallSteps.Undefined)
                    .Error("Error while getting sprints");
                throw;
            }
        } while (!lastPage);

        return sprintList
            .OrderByDescending(x => x.EndDate)
            .ToList();
    }

    public async Task<JiraSprint> GetJiraSprintByIdAsync(int sprintId, JiraBoard? jiraBoard = null)
    {
        var endpoint = _endpointBuilder.Sprints()
            .ForSprint(sprintId)
            .Build();

        _logger
            .WithEndpoint(endpoint)
            .WithAction()
            .WithSprintId(sprintId)
            .WithCallStep(CallSteps.BeforeCall)
            .Debug("");
        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithSprintId(sprintId)
                .WithStatusCode(response.StatusCode)
                .WithCallStep(CallSteps.AfterCall)
                .WithErrorResponse(message)
                .Error("");
            throw new JiraGetSprintByIdException(sprintId, endpoint, response.StatusCode);
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var jiraSprint = JsonSerializer.Deserialize<JiraSprint>(content, _jsonOptions);

        if (jiraSprint == null)
        {
            throw new JiraGetSprintByIdDeserializationException(sprintId, endpoint, response.StatusCode);
        }

        if (jiraBoard == null)
        {
            jiraBoard = await GetBoardByIdAsync(jiraSprint.OriginBoardId);
        }
        jiraSprint.JiraBoard = jiraBoard;
        return jiraSprint;
    }
    
    public async Task<List<JiraIssue>> GetIssuesForSprintAsync(int sprintId)
    {
        var endpoint = _endpointBuilder.Issues()
            .ForSprint(sprintId)
            .Build();

        _logger
            .WithEndpoint(endpoint)
            .WithAction()
            .WithSprintId(sprintId)
            .WithCallStep(CallSteps.BeforeCall)
            .Debug("");
        var response = await _httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithSprintId(sprintId)
                .WithStatusCode(response.StatusCode)
                .WithCallStep(CallSteps.AfterCall)
                .WithErrorResponse(message)
                .Error("");
            throw new JiraGetIssuesForSprintException(sprintId, endpoint, message, response.StatusCode);
        }

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var jiraQueryResponse = JsonSerializer.Deserialize<JiraQueryResponse>(content, _jsonOptions);

        if (jiraQueryResponse?.Issues == null)
        {
            throw new JiraGetIssuesForSprintDeserializationException(sprintId, endpoint, response.StatusCode);
        }

        return jiraQueryResponse.Issues;
    }
}