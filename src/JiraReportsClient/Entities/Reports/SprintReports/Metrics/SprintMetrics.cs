using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient.Entities.Reports.SprintReports.Metrics;

public class SprintMetrics(
    PlannedMetrics planned,
    UnplannedMetrics unplanned,
    TotalMetrics total,
    CancelledMetrics cancelled,
    RemovedMetrics removed)
{
    public PlannedMetrics Planned { get; } = planned ?? throw new ArgumentNullException(nameof(planned));
    public UnplannedMetrics Unplanned { get; } = unplanned ?? throw new ArgumentNullException(nameof(unplanned));
    public TotalMetrics Total { get; } = total ?? throw new ArgumentNullException(nameof(total));
    public CancelledMetrics Cancelled { get; } = cancelled ?? throw new ArgumentNullException(nameof(cancelled));
    public RemovedMetrics Removed { get; } = removed ?? throw new ArgumentNullException(nameof(removed));

    public SprintMetrics(IReadOnlyList<ReportIssue> reportIssues)
        : this(new PlannedMetrics(reportIssues),
            new UnplannedMetrics(reportIssues),
            new TotalMetrics(reportIssues),
            new CancelledMetrics(reportIssues),
            new RemovedMetrics(reportIssues))
    {
    }
    
    public double PercentageTotalDoneFromPlannedByCount => 
        Planned.PlannedCount > 0 ? Total.TotalDoneCount / Planned.PlannedCount * 100 : 0.0;
}