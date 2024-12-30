using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Structs;

namespace JiraReportsClient.Entities.Reports.SprintReports.Metrics;

[DebuggerDisplay("[{ReportType}] {StartIssuesCount} issues, ({StartTimeEstimate.Days}d / {StartTimeEstimate.Hours}h)")]
public abstract class MetricsStart
{
    public ReportIssueType ReportType { get; }
    protected IReadOnlyList<ReportIssue> StartIssues { get; }
    protected ReportIssueType[] ReportTypes { get; }
    protected int StartIssuesCount => StartIssues.Count;
    protected EstimationTime StartTimeEstimate { get; }

    protected MetricsStart(
        IReadOnlyList<ReportIssue> reportIssues,
        params ReportIssueType[] reportTypes)
    {
        ReportTypes = reportTypes;
        ReportType = ReportTypes.Aggregate(ReportIssueType.Unspecified, (a, b) => a | b);
        StartIssues = reportIssues.Where(DefaultStartIssuesFilter).ToList();
        StartTimeEstimate = StartIssues.Sum(EstimationSelector);
    }

    protected static double EstimationSelector(ReportIssue i) => i.EstimationSeconds ?? 0;
    protected bool DefaultStartIssuesFilter(ReportIssue reportIssue) => ReportTypes.Any(reportIssue.HasFlag);
}

[DebuggerDisplay("[{ReportType}] {StartIssuesCount}/{EndIssuesCount}(Done), ({StartTimeEstimate.Days}d, {EndTimeEstimate.Days}d)")]
public abstract class MetricsEnd : MetricsStart
{
    protected IReadOnlyList<ReportIssue> EndIssues { get; }

    protected int EndIssuesCount => EndIssues.Count;
    protected EstimationTime EndTimeEstimate => EndIssues.Sum(EstimationSelector);

    protected MetricsEnd(
        IReadOnlyList<ReportIssue> reportIssues,
        params ReportIssueType[] reportTypes)
        : base(reportIssues, reportTypes)
    {
        EndIssues = reportIssues
            .Where(DefaultStartIssuesFilter)
            .Where(DefaultEndIssuesFilter).ToList();
    }

    protected bool DefaultEndIssuesFilter(ReportIssue reportIssue) =>
        reportIssue.HasFlag(ReportIssueType.Done) || reportIssue.HasFlag(ReportIssueType.Cancelled);

    public FixedDouble PercentageEndByCount => StartIssuesCount > 0 ? (double)EndIssuesCount / StartIssuesCount * 100d : 0.0d;

    public FixedDouble PercentageDoneByTime =>
        StartTimeEstimate > 0 ? (double)EndTimeEstimate / (double)StartTimeEstimate * 100d : 0.0d;
}

public class PlannedMetrics(
    IReadOnlyList<ReportIssue> reportIssues)
    : MetricsEnd(reportIssues, ReportIssueType.Planned)
{
    public int PlannedCount => StartIssuesCount;
    public EstimationTime PlannedTime => StartTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> PlannedIssues => StartIssues;
    public int PlannedDoneCount => EndIssuesCount;
    public EstimationTime PlannedDoneTime => EndTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> PlannedDoneIssues => EndIssues;
    public FixedDouble PlannedDonePercentageByCount => PercentageEndByCount;
    public FixedDouble PlannedDonePercentageByTime => PercentageDoneByTime;
}

public class UnplannedMetrics(
    IReadOnlyList<ReportIssue> reportIssues)
    : MetricsEnd(reportIssues, ReportIssueType.Unplanned)
{
    public int UnplannedCount => StartIssuesCount;
    public EstimationTime UnplannedTime => StartTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> UnplannedIssues => StartIssues;
    public int UnplannedDoneCount => EndIssuesCount;
    public EstimationTime UnplannedDoneTime => EndTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> UnplannedDoneIssues => EndIssues;
    public FixedDouble UnplannedDonePercentageByCount => PercentageEndByCount;
    public FixedDouble UnplannedDonePercentageByTime => PercentageDoneByTime;
}

public class TotalMetrics(
    IReadOnlyList<ReportIssue> reportIssues)
    : MetricsEnd(reportIssues, ReportIssueType.Planned, ReportIssueType.Unplanned)
{
    public int TotalCount => StartIssuesCount;
    public EstimationTime TotalTime => StartTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> TotalIssues => StartIssues;
    public int TotalDoneCount => EndIssuesCount;
    public EstimationTime TotalDoneTime => EndTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> TotalDoneIssues => EndIssues;
    public FixedDouble TotalDonePercentageByCount => PercentageEndByCount;
    public FixedDouble TotalDonePercentageByTime => PercentageDoneByTime;
}

public class CancelledMetrics(
    IReadOnlyList<ReportIssue> reportIssues)
    : MetricsStart(reportIssues, ReportIssueType.Cancelled)
{
    public int CancelledCount => StartIssuesCount;
    public EstimationTime CancelledTime => StartTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> CancelledIssues => StartIssues;
}

public class RemovedMetrics(
    IReadOnlyList<ReportIssue> reportIssues)
    : MetricsStart(reportIssues, ReportIssueType.Removed)
{
    public int RemovedCount => StartIssuesCount;
    public EstimationTime RemovedTime => StartTimeEstimate;
    //[JsonIgnore]
    public IEnumerable<ReportIssue> RemovedIssues => StartIssues;
}