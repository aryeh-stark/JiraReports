using JiraReportsClient.Http.EndpointFluentBuilder;

namespace JiraReportsClientTests.Http.Fluent;

public class IssueBuilderTests
{
    private const string BaseUrl = "https://jira.example.com";
    private readonly JiraEndpointBuilder _builder = new(BaseUrl, 50);

    [Fact]
    public void Build_WithoutIssueIds_ThrowsException()
    {
        var builder = _builder.Issues();
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("At least one issue ID is required", exception.Message);
    }

    [Fact]
    public void Build_WithSingleIssue_ReturnsCorrectUrl()
    {
        var url = _builder.Issues()
            .ForIssues(new[] { "TEST-123" })
            .Build();

        Assert.Equal($"{BaseUrl}/rest/api/3/search?jql=issue in (TEST-123) order by key", url);
    }

    [Fact]
    public void Build_WithMultipleIssues_ReturnsCorrectUrl()
    {
        var url = _builder.Issues()
            .ForIssues(new[] { "TEST-123", "TEST-456", "TEST-789" })
            .Build();

        Assert.Equal($"{BaseUrl}/rest/api/3/search?jql=issue in (TEST-123,TEST-456,TEST-789) order by key", url);
    }
}