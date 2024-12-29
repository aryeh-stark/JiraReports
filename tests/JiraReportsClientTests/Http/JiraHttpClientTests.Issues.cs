using FluentAssertions;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{
    [Fact]
    public async Task GetIssuesByIdAsyncTest()
    {
        var issue = await Client.GetIssueByIdAsync("EW-4401");
        issue.Should().NotBeNull();
        
    }
    
    [Fact]
    public async Task GetIssuesByIdsAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeEmpty();
        
        var lastClosedSprint = sprints
            .Where(s => s.State == "closed")
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefault();
        lastClosedSprint.Should().NotBeNull();
        
        var sprintIssues = await Client.GetIssuesForSprintAsync(lastClosedSprint!.Id);
        sprintIssues.Should().NotBeEmpty();
        
        var issues = await Client.GetIssuesByIdsAsync(sprintIssues.Select(i => i.Key));
        issues.Should().NotBeNullOrEmpty();
    }
}