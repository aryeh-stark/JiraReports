using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Reports.SprintBurndowns;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Http;

namespace JiraReportsClient;

public partial class JiraClient
{
  public async Task<List<Sprint>> GetSprintsForBoardIdAsync(int boardId,
        bool includeFutureSprints = false,
        bool includeActiveSprints = false,
        bool includeClosedSprints = false)
    {
        var jiraSprints = await client.GetSprintsForBoardIdAsync(boardId, includeFutureSprints, includeActiveSprints, includeClosedSprints);
        var sprints = jiraSprints.ToSprintsList();
        return sprints;
    }

    public async Task<Sprint> GetSprintByIdAsync(int sprintId)
    {
        var jiraSprint = await client.GetJiraSprintByIdAsync(sprintId);
        var sprint = jiraSprint.ToModel();
        return sprint;
    }
    
    public async Task<IReadOnlyList<Issue>> GetIssuesForSprintAsync(int sprintId)
    {
        var issues = await client.GetIssuesForSprintAsync(sprintId);
        return issues.ToModelReadOnlyList();
    }
}