using FluentAssertions;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{
    [Fact]
    public async Task TestGetSprintsAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task TestGetIssuesForSprintAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeNullOrEmpty();
        
        var lastSprint = sprints
            .OrderByDescending(s => s.StartDate)
            .First();
        
        var issues = await Client.GetIssuesForSprintAsync(lastSprint.Id);
        issues.Should().NotBeNullOrEmpty();
    }
}