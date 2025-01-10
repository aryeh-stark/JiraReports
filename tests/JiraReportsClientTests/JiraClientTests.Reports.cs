using System.Text.Json;
using FluentAssertions;
using JiraReportsClient.Csv;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Utils.GoogleClient;
using JiraReportsClient.Utils.GoogleClient.Sheets;
using JiraReportsClient.Utils.SchemaGenerators;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    [Fact]
    public async Task GetSprintReportEnrichedAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeEmpty();

        var lastClosedSprint = sprints
            .Where(s => s.State == SprintState.Closed)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();
        lastClosedSprint.Should().NotBeNull();

        var sprintReportEnriched =
            await Client.GetSprintReportEnrichedAsync(lastClosedSprint!.Board, lastClosedSprint!.Id);
        sprintReportEnriched.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSprintReportEnrichedJsonAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeEmpty();

        var lastClosedSprint = sprints
            .Where(s => s.State == SprintState.Closed)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();
        lastClosedSprint.Should().NotBeNull();

        var sprintReportEnriched =
            await Client.GetSprintReportEnrichedAsync(lastClosedSprint!.Board, lastClosedSprint!.Id);
        sprintReportEnriched.Should().NotBeNull();

        var json = sprintReportEnriched.ToJson();
        json.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetSprintReportEnrichedGroupedByUserAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(81);
        sprints.Should().NotBeEmpty();

        var lastClosedSprint = sprints
            .Where(s => s.State == SprintState.Closed)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();
        lastClosedSprint.Should().NotBeNull();

        var sprintReportEnriched =
            await Client.GetSprintReportEnrichedAsync(lastClosedSprint!.Board, lastClosedSprint!.Id);
        sprintReportEnriched.Should().NotBeNull();

        var sprintReportEnrichedGroupedByUser = sprintReportEnriched!.GroupByUser();
        sprintReportEnrichedGroupedByUser.Should().NotBeNullOrEmpty();

        var byUser = UserSprintMetricsCsvSerializer.Serialize(sprintReportEnrichedGroupedByUser.Values);
        var byTeam = SprintMetricsCsvSerializer.Serialize(sprintReportEnriched);
    }

    [Fact]
    public async Task GetAllClosedSprintReportsForBoardNameAsyncTest()
    {
        var sprintReportsByBoardId = await Client.GetAllClosedSprintReportsForBoardNameAsync("BE-Core");
        sprintReportsByBoardId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAllClosedSprintReportsForBoardUserNamesAsyncTest()
    {
        var sprintReportsByBoardUserId = await Client.GetAllClosedSprintReportsForBoardUserNamesAsync(10, "BE-WebSDK");
        sprintReportsByBoardUserId.Should().NotBeEmpty();
        
        
        var googleServiceConfiguration = GoogleServiceConfiguration.LoadFromAppSettings();

        var sprintMetricsSheetsGenerator = new SprintMetricsSheetsGenerator(
            googleServiceConfiguration.HeaderBackgroundColor, googleServiceConfiguration.SprintBackgroundColor);
        sprintMetricsSheetsGenerator.Should().NotBeNull();

        var sheetData = sprintMetricsSheetsGenerator.PrepareSprintMetricsSheetByUser(sprintReportsByBoardUserId);

        var sheetsHelper = new GoogleSheetsHelper(googleServiceConfiguration);
        sheetsHelper.Should().NotBeNull();

        try
        {
            var reference = sheetsHelper.CreateSpreadsheet(sheetData, false);
        }
        catch (Exception e)
        {
            
        }
    }

    [Fact]
    public async Task GetAllClosedSprintReportsForBoardNamesAsyncTest()
    {
        var sprintReportsByBoardId = await Client.GetAllClosedSprintReportsForBoardNamesAsync(12, "BE-Core");
        sprintReportsByBoardId.Should().NotBeEmpty();

        var googleServiceConfiguration = GoogleServiceConfiguration.LoadFromAppSettings();

        var sprintMetricsSheetsGenerator = new SprintMetricsSheetsGenerator(
            googleServiceConfiguration.HeaderBackgroundColor, googleServiceConfiguration.SprintBackgroundColor);
        sprintMetricsSheetsGenerator.Should().NotBeNull();

        var sheetData = sprintMetricsSheetsGenerator.PrepareSprintMetricsSheetByBoard(sprintReportsByBoardId);

        var sheetsHelper = new GoogleSheetsHelper(googleServiceConfiguration);
        sheetsHelper.Should().NotBeNull();

        try
        {
            var reference = sheetsHelper.CreateSpreadsheet(sheetData, false);
        }
        catch (Exception e)
        {
            
        }
    }

    [Fact]
    public async Task GetLastClosedSprintReportsForBoardNamesAsyncTest()
    {
        var sprintReportByBoardId =
            await Client.GetLastClosedSprintReportsForBoardNamesAsync("BE-Core", "BE-Infra", "BE-WebSDK");
        sprintReportByBoardId.Should().NotBeEmpty();

        var googleServiceConfiguration = GoogleServiceConfiguration.LoadFromAppSettings();

        var sprintMetricsSheetsGenerator = new SprintMetricsSheetsGenerator(
            googleServiceConfiguration.HeaderBackgroundColor, googleServiceConfiguration.SprintBackgroundColor);
        sprintMetricsSheetsGenerator.Should().NotBeNull();

        var sheetData = sprintMetricsSheetsGenerator.PrepareSprintMetricsSheetByBoard(sprintReportByBoardId);

        var config = GoogleServiceConfiguration.LoadFromAppSettings();
        config.Should().NotBeNull();

        var helper = new GoogleSheetsHelper(config);
        helper.Should().NotBeNull();

        try
        {
            var reference = helper.CreateSpreadsheet(sheetData);
        }
        catch (Exception e)
        {
        }

        var jsonSchema = JsonSchemaGenerator.GeneratePrettySchema<IReadOnlyDictionary<int, SprintReportEnriched>>();
        var jsonValue = JsonSerializer.Serialize(sprintReportByBoardId);
    }
}