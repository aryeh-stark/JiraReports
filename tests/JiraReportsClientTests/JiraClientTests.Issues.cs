using FluentAssertions;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    [Fact]
    public async Task GetIssuesByIdsAsyncTest()
    {
        var issues = await Client.GetIssuesByIdsAsync(new[] { "EW-1", "EW-2" });
        issues.Should().NotBeNullOrEmpty();
    }
}