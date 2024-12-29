using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient;

public partial class JiraClient
{
    public async Task<IReadOnlyList<Issue>> GetIssuesByIdsAsync(IEnumerable<string> issueIds)
    {
        var jiraIssues = await client.GetIssuesByIdsAsync(issueIds);
        var issues = jiraIssues.ToModelReadOnlyList();
        return issues;
    }
}