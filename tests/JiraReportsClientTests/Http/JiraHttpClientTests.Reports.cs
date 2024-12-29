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
        
        var sprintReportIssues = sprintReport!.Contents.GetAllSprintIssueIds();
        sprintReportIssues.Should().NotBeEmpty();
        
        var issues = await Client.GetIssuesForSprintAsync(lastClosedSprint!.Id);
        issues.Should().NotBeEmpty();
        
        issues.TrueForAll(i => sprintReportIssues.Contains(i.Key)).Should().BeTrue();
        
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
    }
}