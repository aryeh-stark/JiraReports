using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Utils.Structs;

namespace JiraReportsClient.Entities.Reports.SprintReports.Metrics;

public class SprintMetrics(
    Sprint sprint,
    PlannedMetrics planned,
    UnplannedMetrics unplanned,
    TotalMetrics total,
    CancelledMetrics cancelled,
    RemovedMetrics removed)
{
    public string SprintSequenceId => sprint.SequenceId;
    public string BoardName => sprint.Board.Name;
    public PlannedMetrics Planned { get; } = planned ?? throw new ArgumentNullException(nameof(planned));
    public UnplannedMetrics Unplanned { get; } = unplanned ?? throw new ArgumentNullException(nameof(unplanned));
    public TotalMetrics Total { get; } = total ?? throw new ArgumentNullException(nameof(total));
    public CancelledMetrics Cancelled { get; } = cancelled ?? throw new ArgumentNullException(nameof(cancelled));
    public RemovedMetrics Removed { get; } = removed ?? throw new ArgumentNullException(nameof(removed));

    public FixedDouble PercentageTotalDoneFromPlannedByCount { get; }
    public FixedDouble PercentageTotalDoneFromPlannedByTime { get; }

    public SprintMetrics(Sprint sprint, IReadOnlyList<ReportIssue> reportIssues)
        : this(sprint, new PlannedMetrics(reportIssues),
            new UnplannedMetrics(reportIssues),
            new TotalMetrics(reportIssues),
            new CancelledMetrics(reportIssues),
            new RemovedMetrics(reportIssues))
    {
        PercentageTotalDoneFromPlannedByCount = Planned.PlannedCount > 0
            ? (double)Total.TotalDoneCount / (double)Planned.PlannedCount * 100.0d
            : 0.0d;
        PercentageTotalDoneFromPlannedByTime = Planned.PlannedTime > 0
            ? (double)Total.TotalDoneTime / (double)Planned.PlannedTime * 100.0d
            : 0.0d;
    }
}