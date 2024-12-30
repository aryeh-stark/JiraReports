namespace JiraReportsClient.Entities.Reports.SprintReports.Metrics;

public class UserSprintMetricsRecord(string userName, SprintMetrics metrics) : SprintMetricsRecord(metrics)
{
    public string UserName { get; set; } = userName;
}