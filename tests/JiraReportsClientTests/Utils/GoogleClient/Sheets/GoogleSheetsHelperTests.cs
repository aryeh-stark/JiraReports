using Bogus;
using FluentAssertions;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;
using JiraReportsClient.Utils.GoogleClient;
using JiraReportsClient.Utils.GoogleClient.Drive;
using JiraReportsClient.Utils.GoogleClient.Sheets;

namespace JiraReportsClientTests.Utils.GoogleClient.Sheets;

public class GoogleDriveHelperTests
{
    [Fact]
    public void PrepareSprintMetricsSheetMockedTest()
    {
        var config = GoogleServiceConfiguration.LoadFromAppSettings();
        config.Should().NotBeNull();
        
        var sprintMetricsSheetsGenerator = new SprintMetricsSheetsGenerator(config.HeaderBackgroundColor, config.SprintBackgroundColor);
        sprintMetricsSheetsGenerator.Should().NotBeNull();

        var metricsRecord = GenerateBogusMetricsRecord();
        var metricsRecords = new List<SprintMetricsRecord> { metricsRecord };
        const int headerRowCount = 1;
        var sheetData = sprintMetricsSheetsGenerator.PrepareSprintMetricsSheet(metricsRecords);
        var expectedColumnCount =  typeof(SprintMetricsRecord).GetProperties().Length;
        sheetData.Should().NotBeNull();
        sheetData.Title.Should().NotBeNullOrEmpty();
        sheetData.Tabs.Should().NotBeNullOrEmpty();
        sheetData.Tabs.Count.Should().BeGreaterThan(0);
        foreach (var sheetTab in sheetData.Tabs)
        {
            sheetTab.Rows.Should().NotBeNullOrEmpty();
            sheetTab.Rows.Count.Should().Be(metricsRecords.Count + headerRowCount);
            sheetTab.Rows[0].Should().NotBeNullOrEmpty();
            sheetTab.Rows[0].Count.Should().Be(expectedColumnCount - 1);
            sheetTab.Rows[1].Should().NotBeNullOrEmpty();
            sheetTab.Rows[1].Count.Should().Be(expectedColumnCount - 1);
        }
        
       
        
        var helper = new GoogleSheetsHelper(config);
        helper.Should().NotBeNull();
        var sheetReference = helper.CreateSpreadsheet(sheetData);
        sheetReference.Should().NotBeNull();
        
        helper.DeleteSpreadsheet(sheetReference.SheetId);
    }
    
    [Fact]
    public void CreateRowAndAppendRowForSprintMetricsSheetMockedTest()
    {
        var config = GoogleServiceConfiguration.LoadFromAppSettings();
        config.Should().NotBeNull();
        
        var helper = new GoogleSheetsHelper(config);
        helper.Should().NotBeNull();
        
        var sprintMetricsSheetsGenerator = new SprintMetricsSheetsGenerator(config.HeaderBackgroundColor, config.SprintBackgroundColor);
        sprintMetricsSheetsGenerator.Should().NotBeNull();

        var metricsRecord = GenerateBogusMetricsRecord();
        var sheetData = sprintMetricsSheetsGenerator.PrepareSprintMetricsSheet(new List<SprintMetricsRecord> { metricsRecord });
        
        var sheetReference = helper.CreateSpreadsheet(sheetData);
        sheetReference.Should().NotBeNull();
        
        var additionalMetricsRecord = GenerateBogusMetricsRecord();
        var additionalSheetData = sprintMetricsSheetsGenerator.PrepareSprintMetricsSheet(new List<SprintMetricsRecord> { additionalMetricsRecord });
        var additionalSheetReference = helper.AppendToSpreadsheet(sheetReference.SheetId, additionalSheetData);
        additionalSheetReference.Should().NotBeNull();
        
        var drive = new GoogleDriveHelper(config);
        drive.DeleteFile(sheetReference.SheetId);
    }

    private static SprintMetricsRecord GenerateBogusMetricsRecord()
    {
        var faker = new Faker<SprintMetricsRecord>()
            .RuleFor(m => m.SprintSequenceId, f => GenerateSprintCode())
            .RuleFor(m => m.BoardName, _ => "BE-Group")
            .RuleForType(typeof(int), f => f.Random.Number(1, 10))
            .RuleForType(typeof(double), f => (double)f.Random.Number(1, 10));
        return faker.Generate();

        string GenerateSprintCode()
        {
            var year = DateTime.Now.Year.ToString().Substring(2, 2);
            var sprintNumber =
                new Random().Next(1, 26).ToString("D2"); // "D2" ensures leading zero for single-digit numbers
            return $"{year}S{sprintNumber}";
        }
    }
}