using FluentAssertions;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{
    [Fact]
    public async Task TestGetSprintsAsyncTest()
    {
        var jiraSprints = await Client.GetSprintsForBoardIdAsync(84);
        jiraSprints.Should().NotBeNullOrEmpty();
        
        var sprints = jiraSprints.ToSprintsList();
        sprints.Should().NotBeNullOrEmpty();
        
        var jiraSprint = await Client.GetJiraSprintByIdAsync(sprints.First().Id);
        jiraSprint.Should().NotBeNull();
        
        var sprint = jiraSprint.ToModel();
        sprint.Should().NotBeNull();
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