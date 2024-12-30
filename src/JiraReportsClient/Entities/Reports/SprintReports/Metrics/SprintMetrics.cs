using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Utils.Structs;

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
    
    public FixedDouble PercentageTotalDoneFromPlannedByCount => 
        Planned.PlannedCount > 0 ? (double)Total.TotalDoneCount / (double)Planned.PlannedCount * 100.0d : 0.0d;
    
    public FixedDouble PercentageTotalDoneFromPlannedByTime => 
        Planned.PlannedTime > 0 ? Total.TotalDoneTime / Planned.PlannedTime * 100.0d : 0.0d;
}