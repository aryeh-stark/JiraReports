using FluentAssertions;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{
    [Fact]
    public async Task GetSprintReportAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeEmpty();
        
        var lastClosedSprint = sprints
            .Where(s => s.State == "closed")
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();
        lastClosedSprint.Should().NotBeNull();
        
        var sprintReport = await Client.GetSprintReportAsync(84, lastClosedSprint!.Id);
        sprintReport.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetSprintBurndownAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeEmpty();
        
        var lastClosedSprint = sprints
            .Where(s => s.State == "closed")
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();
        lastClosedSprint.Should().NotBeNull();
        
        var sprintBurndown = await Client.GetSprintBurndownAsync(84, lastClosedSprint!.Id);
        sprintBurndown.Should().NotBeNull();

        var s = sprintBurndown!.IssueChanges;
        s.Should().NotBeEmpty();
    }
}