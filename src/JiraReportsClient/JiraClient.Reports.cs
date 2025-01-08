using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Reports.SprintBurndowns;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Http;

namespace JiraReportsClient;

public partial class JiraClient
{
    public async Task<SprintReportEnriched?> GetSprintReportEnrichedAsync(Board? board, int sprintId)
    {
        if (board == null) return null;
        
        // Start the tasks concurrently
        var jiraSprintTask = client.GetJiraSprintByIdAsync(sprintId, board.ToJiraBoardModel());
        var sprintReportTask = client.GetSprintReportAsync(board.Id, sprintId);
        var sprintBurndownTask = client.GetSprintBurndownAsync(board.Id, sprintId);

        // Await all tasks to complete
        await Task.WhenAll(jiraSprintTask, sprintReportTask, sprintBurndownTask);

        // Get results after completion
        var jiraSprint = jiraSprintTask.Result;
        var sprintReportResponse = sprintReportTask.Result;
        var sprintBurndown = sprintBurndownTask.Result;
        
        if (sprintReportResponse?.HasIssues() is not true) return null;

        // Extract issue IDs
        var issueIds = sprintReportResponse?.Contents?.GetAllSprintIssueIds();
        if (issueIds == null || issueIds.Count == 0) return null;

        // Fetch issues by IDs and convert them to models
        var jiraIssues = await client.GetIssuesByIdsAsync(issueIds);
        if (jiraIssues.Count == 0) return null;
        var issues = jiraIssues.Select(i => i.ToModel()).ToList();
            

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