using Google.Apis.Sheets.v4.Data;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;
using JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

namespace JiraReportsClient.Utils.GoogleClient.Sheets;

public class SprintMetricsSheetsGenerator
{
    private static readonly string[] Headers =
    [
        "SprintSequenceId",
        "BoardName",
        "PlannedCount",
        "PlannedDays",
        "PlannedDoneCount",
        "PlannedDoneDays",
        "PlannedDonePercentageByCount",
        "PlannedDonePercentageByTime",
        "UnplannedCount",
        "UnplannedDays",
        "UnplannedDoneCount",
        "UnplannedDoneDays",
        "UnplannedDonePercentageByCount",
        "UnplannedDonePercentageByTime",
        "TotalCount",
        "TotalDays",
        "TotalDoneCount",
        "TotalDoneDays",
        "TotalDonePercentageByCount",
        "TotalDonePercentageByTime",
        "CancelledCount",
        "CancelledDays",
        "RemovedCount",
        "RemovedDays",
        "PercentageTotalDoneFromPlannedByCount",
        "PercentageTotalDoneFromPlannedByTime"
    ];

    private List<SheetCell> CreateHeaderRow()
    {
        var headerFormatting = new SheetFormatting
        {
            IsBold = true,
            BackgroundColor = new Color { Red = 0.8f, Green = 0.8f, Blue = 0.8f }
        };
        return Headers.Select(h => new HeaderCell(h, headerFormatting)).Cast<SheetCell>().ToList();
    }

    private List<SheetCell> CreateDataRow(SprintMetricsRecord record)
    {
        return new List<SheetCell>
        {
            new DataCell(record.SprintSequenceId),
            new DataCell(record.BoardName),
            new DataCell(record.PlannedCount),
            new DataCell(record.PlannedDays),
            new DataCell(record.PlannedDoneCount),
            new DataCell(record.PlannedDoneDays),
            new DataCell(record.PlannedDonePercentageByCount),
            new DataCell(record.PlannedDonePercentageByTime),
            new DataCell(record.UnplannedCount),
            new DataCell(record.UnplannedDays),
            new DataCell(record.UnplannedDoneCount),
            new DataCell(record.UnplannedDoneDays),
            new DataCell(record.UnplannedDonePercentageByCount),
            new DataCell(record.UnplannedDonePercentageByTime),
            new DataCell(record.TotalCount),
            new DataCell(record.TotalDays),
            new DataCell(record.TotalDoneCount),
            new DataCell(record.TotalDoneDays),
            new DataCell(record.TotalDonePercentageByCount),
            new DataCell(record.TotalDonePercentageByTime),
            new DataCell(record.CancelledCount),
            new DataCell(record.CancelledDays),
            new DataCell(record.RemovedCount),
            new DataCell(record.RemovedDays),
            new DataCell(record.PercentageTotalDoneFromPlannedByCount),
            new DataCell(record.PercentageTotalDoneFromPlannedByTime)
        };
    }

    public SheetData PrepareSprintMetricsSheet(IEnumerable<SprintMetricsRecord> records)
    {
        var defaultTab = new SheetTab
        {
            Name = "Sprint Metrics",
            Rows = new List<List<SheetCell>> { CreateHeaderRow() },
        };
    
        defaultTab.Rows.AddRange(records.Select(CreateDataRow));

        return new SheetData
        {
            Title = "Sprint Metrics",
            Tabs = [defaultTab],
        };
    }

    public SheetData PrepareSprintMetricsSheetByBoard(IReadOnlyDictionary<int, SprintReportEnriched> sprintData)
    {
        var sheetsByBoard = sprintData.GroupBy(kv => kv.Value.Board.Name)
            .Select(boardGroup => {
                var headerCells = CreateHeaderRow();
                var dataCells = boardGroup.Select(sprintRecord => 
                    CreateDataRow(new SprintMetricsRecord
                    {
                        SprintSequenceId = sprintRecord.Value.Sprint.SequenceId,
                        BoardName = sprintRecord.Value.Board.Name,
                        PlannedCount = sprintRecord.Value.Metrics.Planned.PlannedCount,
                        PlannedDays = sprintRecord.Value.Metrics.Planned.PlannedTime.Days.Value,
                        PlannedDoneCount = sprintRecord.Value.Metrics.Planned.PlannedDoneCount,
                        PlannedDoneDays = sprintRecord.Value.Metrics.Planned.PlannedDoneTime.Days.Value,
                        PlannedDonePercentageByCount = sprintRecord.Value.Metrics.Planned.PlannedDonePercentageByCount.Value,
                        PlannedDonePercentageByTime = sprintRecord.Value.Metrics.Planned.PlannedDonePercentageByTime.Value,
                        UnplannedCount = sprintRecord.Value.Metrics.Unplanned.UnplannedCount,
                        UnplannedDays = sprintRecord.Value.Metrics.Unplanned.UnplannedTime.Days.Value,
                        UnplannedDoneCount = sprintRecord.Value.Metrics.Unplanned.UnplannedDoneCount,
                        UnplannedDoneDays = sprintRecord.Value.Metrics.Unplanned.UnplannedDoneTime.Days.Value,
                        UnplannedDonePercentageByCount = sprintRecord.Value.Metrics.Unplanned.UnplannedDonePercentageByCount.Value,
                        UnplannedDonePercentageByTime = sprintRecord.Value.Metrics.Unplanned.UnplannedDonePercentageByTime.Value,
                        TotalCount = sprintRecord.Value.Metrics.Total.TotalCount,
                        TotalDays = sprintRecord.Value.Metrics.Total.TotalTime.Days.Value,
                        TotalDoneCount = sprintRecord.Value.Metrics.Total.TotalDoneCount,
                        TotalDoneDays = sprintRecord.Value.Metrics.Total.TotalDoneTime.Days.Value,
                        TotalDonePercentageByCount = sprintRecord.Value.Metrics.Total.TotalDonePercentageByCount.Value,
                        TotalDonePercentageByTime = sprintRecord.Value.Metrics.Total.TotalDonePercentageByTime.Value,
                        CancelledCount = sprintRecord.Value.Metrics.Cancelled.CancelledCount,
                        CancelledDays = sprintRecord.Value.Metrics.Cancelled.CancelledTime.Days.Value,
                        RemovedCount = sprintRecord.Value.Metrics.Removed.RemovedCount,
                        RemovedDays = sprintRecord.Value.Metrics.Removed.RemovedTime.Days.Value,
                        PercentageTotalDoneFromPlannedByCount = sprintRecord.Value.Metrics.PercentageTotalDoneFromPlannedByCount.Value,
                        PercentageTotalDoneFromPlannedByTime = sprintRecord.Value.Metrics.PercentageTotalDoneFromPlannedByTime.Value
                    })).ToList();

                var rows = new List<List<SheetCell>> { headerCells };
                rows.AddRange(dataCells);

                return new SheetTab
                {
                    Name = boardGroup.Key,
                    Rows = rows,
                };
            }).ToList();

        return new SheetData
        {
            Title = "Sprint Metrics",
            Tabs = sheetsByBoard,
        };
    }
}