using FluentAssertions;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Sprints;

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
}