using System.Text.Json;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Logging;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
    public async Task<SprintReport> GetSprintReportAsync(int boardId, int sprintId)
    {
        // Get the sprint report
        var endpoint = _endpointBuilder
            .Reports()
            .ForBoard(boardId)
            .ForSprint(sprintId)
            .BuildSprintReport();
        
        _logger
            .WithEndpoint(endpoint)
            .WithAction()
            .WithBoardId(boardId)
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
                .WithStatusCode(response.StatusCode)
                .WithBoardId(boardId)
                .WithSprintId(sprintId)
                .WithCallStep(CallSteps.AfterCall)
                .Error("");
            throw new JiraGetSprintReportForBoardAndSprintException(message, boardId, sprintId, endpoint, response.StatusCode);
        }

        var content = await response.Content.ReadAsStringAsync();
        var sprintReport = JsonSerializer.Deserialize<SprintReport>(content, _jsonOptions);
        if (sprintReport == null || sprintReport?.Contents == null || sprintReport?.Contents.GetAllSprintIssues().Count == 0)
        {
            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithStatusCode(response.StatusCode)
                .WithBoardId(boardId)
                .WithSprintId(sprintId)
                .WithCallStep(CallSteps.AfterDeserialization)
                .Error($"Content Length: {content.Length}");
            throw new JiraGetSprintReportForBoardAndSprintDeserializationException(boardId, sprintId, endpoint, response.StatusCode);
        }
        
        _logger
            .WithEndpoint(endpoint)
            .WithAction()
            .WithStatusCode(response.StatusCode)
            .WithBoardId(boardId)
            .WithSprintId(sprintId)
            .WithCallStep(CallSteps.AfterCall)
            .Debug($"Content Length: {content.Length}");

        return sprintReport;
    }
}