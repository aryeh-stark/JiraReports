using System.Text.Json;
using FluentAssertions;
using JiraReportsClient.Csv;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Sprints;
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
        
        var sprintReportEnriched = await Client.GetSprintReportEnrichedAsync(84, lastClosedSprint!.Id);
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
        
        var sprintReportEnriched = await Client.GetSprintReportEnrichedAsync(84, lastClosedSprint!.Id);
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
        
        var sprintReportEnriched = await Client.GetSprintReportEnrichedAsync(81, lastClosedSprint!.Id);
        sprintReportEnriched.Should().NotBeNull();

        var p = sprintReportEnriched.Metrics.PercentageTotalDoneFromPlannedByCount;
        
        var sprintReportEnrichedGroupedByUser = sprintReportEnriched.GroupByUser();
        sprintReportEnrichedGroupedByUser.Should().NotBeNullOrEmpty();


        try
        {
            var Roi = UserSprintMetricsCsvSerializer.Serialize(sprintReportEnrichedGroupedByUser.Values);
            var json = JsonSerializer.Serialize(sprintReportEnriched.GroupByUser().Values);
            var schema = JsonSchemaGenerator.GenerateSchema<UserSprintReportEnriched>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}