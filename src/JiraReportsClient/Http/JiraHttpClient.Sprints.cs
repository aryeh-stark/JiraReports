using System.Text.Json;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Http.EndpointFluentBuilder;
using JiraReportsClient.Logging;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
    public async Task<List<Sprint>> GetSprintsForBoardIdAsync(int boardId,
        bool includeFutureSprints = false,
        bool includeActiveSprints = false,
        bool includeClosedSprints = false)
    {
        var (isScrumBoard, board) = await IsBoardScrum(boardId);
        if (isScrumBoard == false)
            throw new JiraNotScrumBoardException(boardId);

        bool isLastPage = false;
        var sprintsEndpointBuilder = _endpointBuilder.Sprints()
            .ForBoard(boardId)
            .IncludeStates(future: includeFutureSprints, active: includeActiveSprints, closed: includeClosedSprints)
            .WithPagination();
        var sprintList = new List<Sprint>();
        bool lastPage = false;
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
                var sprintsResponse = JsonSerializer.Deserialize<SprintResponse>(content, _jsonOptions);

                if (sprintsResponse == null || !sprintsResponse.HasValues())
                    throw new JiraGetSprintsForBoardDeserializationException(boardId, endpoint, response.StatusCode);

                sprintList.AddRange(sprintsResponse.Values.Select(s =>
                {
                    s.Board = board!;
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
                isLastPage = lastPage;
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
        } while (!isLastPage);

        return sprintList
            .OrderByDescending(x => x.EndDate)
            .ToList();
    }

    public async Task<Sprint> GetJiraSprintAsync(int sprintId)
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
        var sprint = JsonSerializer.Deserialize<Sprint>(content, _jsonOptions);

        if (sprint == null)
        {
            throw new JiraGetSprintByIdDeserializationException(sprintId, endpoint, response.StatusCode);
        }

        return sprint;
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