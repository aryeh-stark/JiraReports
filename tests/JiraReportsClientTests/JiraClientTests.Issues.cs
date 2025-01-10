using FluentAssertions;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    [Fact]
    public async Task GetIssuesByIdsAsyncTest()
    {
        var issues = await Client.GetIssuesByIdsAsync(new[] { "EW-1103", "EW-1215" });
        issues.Should().NotBeNullOrEmpty();
    }
}