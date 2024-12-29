using FluentAssertions;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    [Fact]
    public async Task GetProjectsAsyncTest()
    {
        var projects = await Client.GetProjectsAsync();
        projects.Should().NotBeNullOrEmpty();
    }
}