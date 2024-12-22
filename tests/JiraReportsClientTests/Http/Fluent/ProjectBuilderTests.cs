using JiraReportsClient.Http.EndpointFluentBuilder;

namespace JiraReportsClientTests.Http.Fluent;

public class ProjectBuilderTests
{
    private const string BaseUrl = "https://jira.example.com";
    private readonly JiraEndpointBuilder _builder = new(BaseUrl, 50);

    [Fact]
    public void Build_ReturnsCorrectUrl()
    {
        var url = _builder.Projects().Build();
        Assert.Equal($"{BaseUrl}/rest/api/3/project/search?startAt=0&maxResults=50&orderBy=name", url);
    }

    [Fact]
    public void WithPagination_SetsCorrectStartAtAndMaxResults()
    {
        var url = _builder.Projects()
            .WithPagination(100, 25)
            .Build();

        Assert.Equal($"{BaseUrl}/rest/api/3/project/search?startAt=100&maxResults=25&orderBy=name", url);
    }

    [Fact]
    public void BuildNextPage_ReturnsCorrectUrl()
    {
        var builder = _builder.Projects()
            .WithPagination(0, 25);

        var nextPage = builder.BuildNextPage();
        Assert.Equal($"{BaseUrl}/rest/api/3/project/search?startAt=0&maxResults=25&orderBy=name", nextPage);
    }

    [Fact]
    public void BuildPages_ReturnsCorrectNumberOfPages()
    {
        var pages = _builder.Projects()
            .WithPagination(0, 25)
            .BuildPages(3)
            .ToList();
        
        Assert.Equal(3, pages.Count);
        Assert.Equal($"{BaseUrl}/rest/api/3/project/search?startAt=0&maxResults=25&orderBy=name", pages[0]);
        Assert.Equal($"{BaseUrl}/rest/api/3/project/search?startAt=25&maxResults=25&orderBy=name", pages[1]);
        Assert.Equal($"{BaseUrl}/rest/api/3/project/search?startAt=50&maxResults=25&orderBy=name", pages[2]);
    }
}