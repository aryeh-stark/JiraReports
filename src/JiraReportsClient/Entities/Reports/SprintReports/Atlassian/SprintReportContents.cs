using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

public class SprintReportContents
{
    public HashSet<string> GetAllSprintIssueIds()
    {
        var allIssueIds = GetAllSprintIssues()
            .Select(i => i.Key)
            .ToHashSet();
        return allIssueIds;
    }
    public List<SprintIssueDetails> GetAllSprintIssues()
    {
        var allIssues =
            CompletedIssues
                .Concat(IssuesNotCompletedInCurrentSprint)
                .Concat(PuntedIssues)
                .ToList();
        return allIssues;
    }

    public bool HasIssues()
    {
        return CompletedIssues.Count != 0
               || IssuesNotCompletedInCurrentSprint.Count != 0
               || PuntedIssues.Count != 0;
    }


    [JsonPropertyName("completedIssues")]
    public List<SprintIssueDetails> CompletedIssues { get; set; }

    [JsonPropertyName("issuesNotCompletedInCurrentSprint")]
    public List<SprintIssueDetails> IssuesNotCompletedInCurrentSprint { get; set; }

    [JsonPropertyName("puntedIssues")]
    public List<SprintIssueDetails> PuntedIssues { get; set; }

    [JsonPropertyName("issuesCompletedInAnotherSprint")]
    public List<SprintIssueDetails> IssuesCompletedInAnotherSprint { get; set; }

    [JsonPropertyName("completedIssuesInitialEstimateSum")]
    public EstimateSum CompletedIssuesInitialEstimateSum { get; set; }

    [JsonPropertyName("completedIssuesEstimateSum")]
    public EstimateSum CompletedIssuesEstimateSum { get; set; }

    [JsonPropertyName("issuesNotCompletedInitialEstimateSum")]
    public EstimateSum IssuesNotCompletedInitialEstimateSum { get; set; }

    [JsonPropertyName("issuesNotCompletedEstimateSum")]
    public EstimateSum IssuesNotCompletedEstimateSum { get; set; }

    [JsonPropertyName("allIssuesEstimateSum")]
    public EstimateSum AllIssuesEstimateSum { get; set; }

    [JsonPropertyName("puntedIssuesInitialEstimateSum")]
    public EstimateSum PuntedIssuesInitialEstimateSum { get; set; }

    [JsonPropertyName("puntedIssuesEstimateSum")]
    public EstimateSum PuntedIssuesEstimateSum { get; set; }

    [JsonPropertyName("issuesCompletedInAnotherSprintInitialEstimateSum")]
    public EstimateSum IssuesCompletedInAnotherSprintInitialEstimateSum { get; set; }

    [JsonPropertyName("issuesCompletedInAnotherSprintEstimateSum")]
    public EstimateSum IssuesCompletedInAnotherSprintEstimateSum { get; set; }

    [JsonPropertyName("issueKeysAddedDuringSprint")]
    public Dictionary<string, bool> IssueKeysAddedDuringSprint { get; set; }
}