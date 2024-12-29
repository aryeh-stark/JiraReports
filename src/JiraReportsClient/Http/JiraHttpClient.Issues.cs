using System.Text.Json;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Issues.Atlassian;
using JiraReportsClient.Logging;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
    public async Task<JiraIssue?> GetIssueByIdAsync(string issueId)
    {
        if (string.IsNullOrWhiteSpace(issueId))
        {
            throw new ArgumentNullException(nameof(issueId));
        }
        
        var endpoint = _endpointBuilder
            .Issues()
            .ForIssue(issueId)
            .Build();

        _logger
            .WithEndpoint(endpoint)
            .WithAction()
            .WithCallStep(CallSteps.BeforeCall)
            .WithIssueKey(issueId)
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
                .WithErrorResponse(message)
                .Error("");
            throw new JiraGetIssueByIdException(message, issueId, endpoint, response.StatusCode);
        }
        var content = await response.Content.ReadAsStringAsync();
        var jiraIssue = JsonSerializer.Deserialize<JiraIssue>(content, _jsonOptions);

        _logger
            .WithEndpoint(endpoint)
            .WithStatusCode(response.StatusCode)
            .WithAction()
            .WithCallStep(CallSteps.AfterCall)
            .Debug($"Received issue: {jiraIssue?.Key}");
        
        return jiraIssue;
    }
    
    public async Task<List<JiraIssue>> GetIssuesByIdsAsync(IEnumerable<string> issueIds)
    {
        if (issueIds == null || !issueIds.Any())
        {
            throw new ArgumentNullException(nameof(issueIds));
        }
        
        var batchedIssueIds = issueIds
            .Distinct()
            .OrderBy(i => i)
            .Chunk(ChunkSize)
            .ToList();
        
        var jiraSprintList = new List<JiraIssue>();
        var endpointBuilder = _endpointBuilder
            .Issues();
            

        foreach (var batchedIssues in batchedIssueIds)
        {
            var endpoint = endpointBuilder
                .ForIssues(batchedIssues)
                .Build();

            _logger
                .WithEndpoint(endpoint)
                .WithAction()
                .WithCallStep(CallSteps.BeforeCall)
                .WithIssueKeys(batchedIssues)
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
                    .WithErrorResponse(message)
                    .Error("");
                throw new JiraGetIssuesByIdsException(message, issueIds, batchedIssues, endpoint, response.StatusCode);
            }
            var content = await response.Content.ReadAsStringAsync();
            var jiraQueryResponse = JsonSerializer.Deserialize<JiraQueryResponse>(content, _jsonOptions);

            if (jiraQueryResponse?.Issues != null)
            {
                jiraSprintList.AddRange(jiraQueryResponse.Issues);
            }
            
            _logger
                .WithEndpoint(endpoint)
                .WithStatusCode(response.StatusCode)
                .WithAction()
                .WithCallStep(CallSteps.AfterCall)
                .Debug($"Received {jiraQueryResponse?.Issues?.Count} issues. Total Received so far: {jiraSprintList.Count}");
        }

        return jiraSprintList;
    }
}