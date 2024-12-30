namespace JiraReportsClient.Entities.Reports.SprintReports.Metrics;

public class SprintMetricsRecord
{
    public string SprintSequenceId { get; set; }
    public string BoardName { get; set; }
    public int PlannedCount { get; set; }
    public double PlannedDays { get; set; }
    public int PlannedDoneCount { get; set; }
    public double PlannedDoneDays { get; set; }
    public double PlannedDonePercentageByCount { get; set; }
    public double PlannedDonePercentageByTime { get; set; }
    public int UnplannedCount { get; set; }
    public double UnplannedDays { get; set; }
    public int UnplannedDoneCount { get; set; }
    public double UnplannedDoneDays { get; set; }
    public double UnplannedDonePercentageByCount { get; set; }
    public double UnplannedDonePercentageByTime { get; set; }
    public int TotalCount { get; set; }
    public double TotalDays { get; set; }
    public int TotalDoneCount { get; set; }
    public double TotalDoneDays { get; set; }
    public double TotalDonePercentageByCount { get; set; }
    public double TotalDonePercentageByTime { get; set; }
    public int CancelledCount { get; set; }
    public double CancelledDays { get; set; }
    public int RemovedCount { get; set; }
    public double RemovedDays { get; set; }
    public double PercentageTotalDoneFromPlannedByCount { get; set; }
    public double PercentageTotalDoneFromPlannedByTime { get; set; }

    public SprintMetricsRecord(SprintMetrics metrics)
    {
        SprintSequenceId = metrics.SprintSequenceId;
        BoardName = metrics.BoardName;
        PlannedCount = metrics.Planned.PlannedCount;
        PlannedDays = metrics.Planned.PlannedTime?.Days?.Value ?? 0;
        PlannedDoneCount = metrics.Planned.PlannedDoneCount;
        PlannedDoneDays = metrics.Planned.PlannedDoneTime?.Days?.Value ?? 0;
        PlannedDonePercentageByCount = metrics.Planned.PlannedDonePercentageByCount?.Value ?? 0;
        PlannedDonePercentageByTime = metrics.Planned.PlannedDonePercentageByTime?.Value ?? 0;
        UnplannedCount = metrics.Unplanned.UnplannedCount;
        UnplannedDays = metrics.Unplanned.UnplannedTime?.Days?.Value ?? 0;
        UnplannedDoneCount = metrics.Unplanned.UnplannedDoneCount;
        UnplannedDoneDays = metrics.Unplanned.UnplannedDoneTime?.Days?.Value ?? 0;
        UnplannedDonePercentageByCount = metrics.Unplanned.UnplannedDonePercentageByCount?.Value ?? 0;
        UnplannedDonePercentageByTime = metrics.Unplanned.UnplannedDonePercentageByTime?.Value ?? 0;
        TotalCount = metrics.Total.TotalCount;
        TotalDays = metrics.Total.TotalTime?.Days?.Value ?? 0;
        TotalDoneCount = metrics.Total.TotalDoneCount;
        TotalDoneDays = metrics.Total.TotalDoneTime?.Days?.Value ?? 0;
        TotalDonePercentageByCount = metrics.Total.TotalDonePercentageByCount?.Value ?? 0;
        TotalDonePercentageByTime = metrics.Total.TotalDonePercentageByTime?.Value ?? 0;
        CancelledCount = metrics.Cancelled.CancelledCount;
        CancelledDays = metrics.Cancelled.CancelledTime?.Days?.Value ?? 0;
        RemovedCount = metrics.Removed.RemovedCount;
        RemovedDays = metrics.Removed.RemovedTime?.Days?.Value ?? 0;
        PercentageTotalDoneFromPlannedByCount = metrics.PercentageTotalDoneFromPlannedByCount;
        PercentageTotalDoneFromPlannedByTime= metrics.PercentageTotalDoneFromPlannedByTime;
    }
}