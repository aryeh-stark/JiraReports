using System.Text;
using JiraReportsClient.Entities.Reports.SprintReports;

namespace JiraReportsClient.Csv;

public static class SprintMetricsCsvSerializer 
{
    public static string Serialize(IEnumerable<SprintReportEnriched> sprintReports)
    {
        var csv = new StringBuilder();
        
        // Headers
        csv.AppendLine("Sprint," +
                       "Planned Count,Planned Days,Planned Done Count,Planned Done Days,Planned Done % (Count),Planned Done % (Time)," +
                       "Unplanned Count,Unplanned Days,Unplanned Done Count,Unplanned Done Days,Unplanned Done % (Count),Unplanned Done % (Time)," +
                       "Total Count,Total Days,Total Done Count,Total Done Days,Total Done % (Count),Total Done % (Time)," +
                       "Cancelled Count,Cancelled Days," +
                       "Removed Count,Removed Days," +
                       "Total Done from Planned % (Count),Total Done from Planned % (Time)");

        foreach (var report in sprintReports)
        {
            var metrics = report.Metrics;
            var sprint = report.Sprint?.Name ?? "Unknown";

            csv.AppendLine(
                $"{sprint}," +
                // Planned
                $"{metrics.Planned.PlannedCount}," +
                $"{metrics.Planned.PlannedTime?.Days?.Value ?? 0:F2}," +
                $"{metrics.Planned.PlannedDoneCount}," +
                $"{metrics.Planned.PlannedDoneTime?.Days?.Value ?? 0:F2}," +
                $"{metrics.Planned.PlannedDonePercentageByCount?.Value ?? 0:F2}," +
                $"{metrics.Planned.PlannedDonePercentageByTime?.Value ?? 0:F2}," +
                // Unplanned
                $"{metrics.Unplanned.UnplannedCount}," +
                $"{metrics.Unplanned.UnplannedTime?.Days?.Value ?? 0:F2}," +
                $"{metrics.Unplanned.UnplannedDoneCount}," +
                $"{metrics.Unplanned.UnplannedDoneTime?.Days?.Value ?? 0:F2}," +
                $"{metrics.Unplanned.UnplannedDonePercentageByCount?.Value ?? 0:F2}," +
                $"{metrics.Unplanned.UnplannedDonePercentageByTime?.Value ?? 0:F2}," +
                // Total
                $"{metrics.Total.TotalCount}," +
                $"{metrics.Total.TotalTime?.Days?.Value ?? 0:F2}," +
                $"{metrics.Total.TotalDoneCount}," +
                $"{metrics.Total.TotalDoneTime?.Days?.Value ?? 0:F2}," +
                $"{metrics.Total.TotalDonePercentageByCount?.Value ?? 0:F2}," +
                $"{metrics.Total.TotalDonePercentageByTime?.Value ?? 0:F2}," +
                // Cancelled
                $"{metrics.Cancelled.CancelledCount}," +
                $"{metrics.Cancelled.CancelledTime?.Days?.Value ?? 0:F2}," +
                // Removed
                $"{metrics.Removed.RemovedCount}," +
                $"{metrics.Removed.RemovedTime?.Days?.Value ?? 0:F2}," +
                // Total Done from Planned
                $"{metrics.PercentageTotalDoneFromPlannedByCount:F2}," +
                $"{metrics.PercentageTotalDoneFromPlannedByTime:F2}");
        }

        return csv.ToString();
    }
}