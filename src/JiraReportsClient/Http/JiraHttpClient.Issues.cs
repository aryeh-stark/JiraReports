using System.Text.Json;
using JiraReportsClient.Entities.Jql;
using JiraReportsClient.Logging;

namespace JiraReportsClient.Http;

public partial class JiraHttpClient
{
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