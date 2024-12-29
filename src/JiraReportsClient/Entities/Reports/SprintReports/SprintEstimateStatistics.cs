namespace JiraReportsClient.Entities.Reports.SprintReports;

public class SprintEstimateStatistics
{
    public double? CompletedIssuesInitialEstimate { get; set; }
    public double? CompletedIssuesFinalEstimate { get; set; }

    public double? NotCompletedInitialEstimate { get; set; }
    public double? NotCompletedFinalEstimate { get; set; }

    public double? AllIssuesEstimate { get; set; }

    public double? PuntedIssuesInitialEstimate { get; set; }
    public double? PuntedIssuesFinalEstimate { get; set; }

    public double? IssuesCompletedAnotherSprintInitialEstimate { get; set; }
    public double? IssuesCompletedAnotherSprintFinalEstimate { get; set; }
}