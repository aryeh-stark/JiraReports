using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Reports.SprintBurndowns;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Http;

namespace JiraReportsClient;

public partial class JiraClient
{
    public async Task<SprintReportEnriched?> GetSprintReportEnrichedAsync(int boardId, int sprintId)
    {
        // Start the tasks concurrently
        var jiraSprintTask = client.GetJiraSprintByIdAsync(sprintId);
        var sprintReportTask = client.GetSprintReportAsync(boardId, sprintId);
        var sprintBurndownTask = client.GetSprintBurndownAsync(boardId, sprintId);

        // Await all tasks to complete
        await Task.WhenAll(jiraSprintTask, sprintReportTask, sprintBurndownTask);

        // Get results after completion
        var jiraSprint = jiraSprintTask.Result;
        var sprintReportResponse = sprintReportTask.Result;
        var sprintBurndown = sprintBurndownTask.Result;

        // Extract issue IDs
        var issueIds = sprintReportResponse?.Contents.GetAllSprintIssueIds();

        // Fetch issues by IDs and convert them to models
        var issues = await client.GetIssuesByIdsAsync(issueIds)
            .ContinueWith(issuesList => issuesList.Result.Select(i => i.ToModel()).ToList());

        // Create dictionaries and process results
        var issueByKey = issues.ToDictionary(i => i.Key);
        var issueChangeByKey = sprintBurndown.ExtractIssueChanges();

        // Build the enriched sprint report
        var sprintReport = new SprintReportEnriched(
            jiraSprint,
            issues,
            sprintReportResponse?.Contents.CompletedIssues.Select(i => issueByKey[i.Key]).ToList() ?? [],
            sprintReportResponse?.Contents.IssuesNotCompletedInCurrentSprint.Select(i => issueByKey[i.Key]).ToList() ?? [],
            sprintReportResponse?.Contents.PuntedIssues.Select(i => issueByKey[i.Key]).ToList() ?? [],
            issueChangeByKey
        );

        return sprintReport;
    }
}