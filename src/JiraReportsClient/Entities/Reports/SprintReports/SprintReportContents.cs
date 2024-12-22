using System.Collections.Generic;
using System.Text.Json.Serialization;
using JiraCmdLineTool.Common.Objects.Outputs;

namespace JiraCmdLineTool.Common.Objects.SprintReports;

public class SprintReportContents
{
    public List<string> GetAllSprintIssueIds()
    {
        return GetAllSprintIssues()
            .Select(i => i.Key)
            .OrderBy(x => x)
            .ToList();
    }
    
    public List<SprintIssue> GetAllSprintIssues()
    {
        var allIssues = new List<SprintIssue>();
        allIssues.AddRange(CompletedIssues);
        allIssues.AddRange(IssuesNotCompletedInCurrentSprint);
        allIssues.AddRange(PuntedIssues);
        allIssues.AddRange(IssuesCompletedInAnotherSprint);
        return allIssues
            .OrderBy(i => i.Key)
            .Distinct()
            .ToList();
    }

    public string GetReportStatus(string issueKey)
    {
        if (CompletedIssues.Any(i => i.Key == issueKey))
        {
            return ReportStatusValues.CompletedIssues;
        }

        if (IssuesNotCompletedInCurrentSprint.Any(i => i.Key == issueKey))
        {
            return ReportStatusValues.IssuesNotCompleted;
        }

        if (PuntedIssues.Any(i => i.Key == issueKey))
        {
            return ReportStatusValues.IssuesRemovedFromSprint;
        }

        if (IssuesCompletedInAnotherSprint.Any(i => i.Key == issueKey))
        {
            return ReportStatusValues.IssuesCompletedOutsideOfThisSprint;
        }

        return "Not Found";
    }

    [JsonPropertyName("completedIssues")]
    public List<SprintIssue> CompletedIssues { get; set; }

    [JsonPropertyName("issuesNotCompletedInCurrentSprint")]
    public List<SprintIssue> IssuesNotCompletedInCurrentSprint { get; set; }

    [JsonPropertyName("puntedIssues")]
    public List<SprintIssue> PuntedIssues { get; set; }

    [JsonPropertyName("issuesCompletedInAnotherSprint")]
    public List<SprintIssue> IssuesCompletedInAnotherSprint { get; set; }

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