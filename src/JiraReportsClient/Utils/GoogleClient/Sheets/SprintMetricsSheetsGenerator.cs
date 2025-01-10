using Google.Apis.Sheets.v4.Data;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;
using JiraReportsClient.Utils.GoogleClient.Sheets.Entities;

namespace JiraReportsClient.Utils.GoogleClient.Sheets;

public class SprintMetricsSheetsGenerator(Color backgroundHeaderColor, Color backgroundSprintColor)
{
    private Color _backgroundHeaderColor = backgroundHeaderColor;
    
    private static readonly string[] Headers =
    [
        "SprintSequenceId",
        //"BoardName",
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
            BackgroundColor = _backgroundHeaderColor,
        };
        return Headers.Select(h => new HeaderCell(h, headerFormatting)).Cast<SheetCell>().ToList();
    }

    private List<SheetCell> CreateDataRow(SprintMetricsRecord record)
    {
        return new List<SheetCell>
        {
            new DataCell(record.SprintSequenceId, backgroundSprintColor),
            //new DataCell(record.BoardName),
            new DataCell(record.PlannedCount),
            new DataCell(record.PlannedDays),
            new DataCell(record.PlannedDoneCount),
            new DataCell(record.PlannedDoneDays),
            new DataCell(record.PlannedDonePercentageByCount, SheetFormatting.BoldFont),
            new DataCell(record.PlannedDonePercentageByTime, SheetFormatting.BoldFont),
            new DataCell(record.UnplannedCount),
            new DataCell(record.UnplannedDays),
            new DataCell(record.UnplannedDoneCount),
            new DataCell(record.UnplannedDoneDays),
            new DataCell(record.UnplannedDonePercentageByCount, SheetFormatting.BoldFont),
            new DataCell(record.UnplannedDonePercentageByTime, SheetFormatting.BoldFont),
            new DataCell(record.TotalCount),
            new DataCell(record.TotalDays),
            new DataCell(record.TotalDoneCount),
            new DataCell(record.TotalDoneDays),
            new DataCell(record.TotalDonePercentageByCount, SheetFormatting.BoldFont),
            new DataCell(record.TotalDonePercentageByTime, SheetFormatting.BoldFont),
            new DataCell(record.CancelledCount),
            new DataCell(record.CancelledDays),
            new DataCell(record.RemovedCount),
            new DataCell(record.RemovedDays),
            new DataCell(record.PercentageTotalDoneFromPlannedByCount, SheetFormatting.BoldFont),
            new DataCell(record.PercentageTotalDoneFromPlannedByTime, SheetFormatting.BoldFont)
        };
    }

    public SheetData PrepareSprintMetricsSheet(IEnumerable<SprintMetricsRecord> records)
    {
        if (records == null || records.Any() is false) 
            throw new ArgumentException("No records to process");
        
        var tabName = records
            .Select(r => r.BoardName)
            .GroupBy(b => b)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault() ?? "Sprint Metrics";
            
        var defaultTab = new SheetTab
        {
            Name = "Sprint Metrics",
            Rows = new List<List<SheetCell>> { CreateHeaderRow() },
        };

        defaultTab.Rows.AddRange(records
            .OrderBy(r => r.SprintSequenceId)
            .Select(CreateDataRow));

        return new SheetData
        {
            Title = tabName,
            Tabs = [defaultTab],
        };
    }

    public SheetData PrepareSprintMetricsSheetByBoard(
        IReadOnlyDictionary<BoardRecord, IEnumerable<SprintReportEnriched>> sprintsData)
    {
        var sheetTabsByBoard = sprintsData
            .Select(boardSprints =>
            {
                var headerCells = CreateHeaderRow();
                var dataCells = boardSprints.Value
                    .OrderBy(s => s.Sprint.Id)
                    .Select(sprintRecord => CreateDataRow(new SprintMetricsRecord(sprintRecord.Metrics)))
                    .ToList();

                var rows = new List<List<SheetCell>> { headerCells };
                rows.AddRange(dataCells);

                return new SheetTab
                {
                    Name = boardSprints.Key.BoardName,
                    Rows = rows,
                };
            })
            .ToList();
        var sheetData = new SheetData
        {
            Title = "Sprint Metrics",
            Tabs = sheetTabsByBoard,
        };
        return sheetData;
    }

    public SheetData PrepareSprintMetricsSheetByBoard(IReadOnlyDictionary<BoardRecord, SprintReportEnriched> sprintData)
    {
        var sheetsByBoard = sprintData
            .Select(board =>
            {
                var headerCells = CreateHeaderRow();
                var dataCells = CreateDataRow(new SprintMetricsRecord(board.Value.Metrics));

                var rows = new List<List<SheetCell>> { headerCells };
                rows.AddRange(dataCells);

                return new SheetTab
                {
                    Name = board.Key.BoardName,
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