using FluentAssertions;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    [Fact]
    public async Task GetIssuesForSprintAsyncTest()
    {
        
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeNullOrEmpty();
    
        var lastSprint = sprints
            .OrderByDescending(s => s.StartDate)
            .First();
    
        var issues = await Client.GetIssuesForSprintAsync(lastSprint.Id);
        issues.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetSprintsForBoardIdAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetJiraSprintByIdAsyncTest()
    {
        var sprints = await Client.GetSprintsForBoardIdAsync(84);
        sprints.Should().NotBeNullOrEmpty();
        
        var sprint = await Client.GetSprintByIdAsync(sprints.First().Id);
        sprint.Should().NotBeNull();
    }

}