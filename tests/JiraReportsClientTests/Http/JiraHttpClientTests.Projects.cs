using FluentAssertions;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{
    [Fact]
    public async Task GetProjectsAsyncTest()
    {
        var projects = await Client.GetProjectsAsync();
        projects.Should().NotBeNullOrEmpty();
    }
}