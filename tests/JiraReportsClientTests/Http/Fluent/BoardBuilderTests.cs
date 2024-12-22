using JiraReportsClient.Http.EndpointFluentBuilder;

namespace JiraReportsClientTests.Http.Fluent;

public class BoardBuilderTests
{
    private const string BaseUrl = "https://jira.example.com";
    private readonly JiraEndpointBuilder _builder = new(BaseUrl, 50);


    [Fact]
    public void Build_WithProjectKey_ReturnsCorrectUrl()
    {
        var url = _builder.Boards()
            .ForProject("TEST")
            .Build();
            
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board?projectKeyOrId=TEST&startAt=0&maxResults=50", url);
    }

    [Fact]
    public void WithPagination_SetsCorrectStartAtAndMaxResults()
    {
        var url = _builder.Boards()
            .ForProject("TEST")
            .WithPagination(100, 25)
            .Build();

        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board?projectKeyOrId=TEST&startAt=100&maxResults=25", url);
    }

    [Fact]
    public void BuildNextPage_ReturnsCorrectUrl()
    {
        var builder = _builder.Boards()
            .ForProject("TEST")
            .WithPagination(0, 25);

        var nextPage = builder.BuildNextPage();
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board?projectKeyOrId=TEST&startAt=0&maxResults=25", nextPage);
    }

    [Fact]
    public void BuildPages_ReturnsCorrectNumberOfPages()
    {
        var pages = _builder.Boards()
            .ForProject("TEST")
            .WithPagination(0, 25)
            .BuildPages(3)
            .ToList();
        
        Assert.Equal(3, pages.Count);
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board?projectKeyOrId=TEST&startAt=0&maxResults=25", pages[0]);
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board?projectKeyOrId=TEST&startAt=25&maxResults=25", pages[1]);
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board?projectKeyOrId=TEST&startAt=50&maxResults=25", pages[2]);
    }
}