using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;

namespace JiraReportsClient.Entities.Reports.SprintReports.Users;

public class UserSprintReport
{
    public User User { get; set; }
    public List<ReportIssue> AssignedIssues { get; set; }
    public EstimationTime TotalEstimation { get; set; }
    public double CompletionRate { get; set; }
    public List<ReportIssue> CompletedIssues { get; set; }
    public List<ReportIssue> InProgressIssues { get; set; }
    public bool HasUnplannedWork { get; set; }
    public EstimationTime UnplannedWorkEstimation { get; set; }
    public EstimationTime PlannedWorkEstimation { get; set; }
}