using JiraReportsClient.Http.EndpointFluentBuilder;

namespace JiraReportsClientTests.Http.Fluent;

public class SprintBuilderTests
{
    private const string BaseUrl = "https://jira.example.com";
    private readonly JiraEndpointBuilder _builder = new(BaseUrl, 50);

    [Fact]
    public void Build_WithSprintId_ReturnsCorrectUrl()
    {
        var url = _builder.Sprints()
            .ForSprint(123)
            .Build();
            
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/sprint/123", url);
    }

    [Fact]
    public void Build_WithoutBoardId_ThrowsException()
    {
        var builder = _builder.Sprints();
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Board ID is required", exception.Message);
    }

    [Fact]
    public void Build_WithBoardIdOnly_ReturnsUrlWithoutStates()
    {
        var url = _builder.Sprints()
            .ForBoard(456)
            .Build();
            
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board/456/sprint?startAt=0&maxResults=50", url);
    }

    [Theory]
    [InlineData(true, false, false, "future")]
    [InlineData(false, true, false, "active")]
    [InlineData(false, false, true, "closed")]
    [InlineData(true, true, true, "future,active,closed")]
    [InlineData(true, false, true, "future,closed")]
    public void Build_WithStates_ReturnsCorrectStateQuery(bool future, bool active, bool closed, string expectedStates)
    {
        var url = _builder.Sprints()
            .ForBoard(456)
            .IncludeStates(future, active, closed)
            .Build();

        var expected = $"{BaseUrl}/rest/agile/1.0/board/456/sprint?startAt=0&maxResults=50&state={expectedStates}";
        Assert.Equal(expected, url);
    }

    [Fact]
    public void WithPagination_SetsCorrectStartAtAndMaxResults()
    {
        var url = _builder.Sprints()
            .ForBoard(456)
            .WithPagination(100, 25)
            .Build();

        var expected = $"{BaseUrl}/rest/agile/1.0/board/456/sprint?startAt=100&maxResults=25";
        Assert.Equal(expected, url);
    }

    [Fact]
    public void BuildNextPage_ReturnsCorrectUrl()
    {
        var builder = _builder.Sprints()
            .ForBoard(456)
            .WithPagination(0, 25);

        var nextPage = builder.BuildNextPage();
        var expected = $"{BaseUrl}/rest/agile/1.0/board/456/sprint?startAt=0&maxResults=25";
        Assert.Equal(expected, nextPage);
    }

    [Fact]
    public void BuildPages_ReturnsCorrectNumberOfPages()
    {
        var pages = _builder.Sprints()
            .ForBoard(456)
            .WithPagination(0, 25)
            .BuildPages(3)
            .ToList();
        
        Assert.Equal(3, pages.Count);
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board/456/sprint?startAt=0&maxResults=25", pages[0]);
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board/456/sprint?startAt=25&maxResults=25", pages[1]);
        Assert.Equal($"{BaseUrl}/rest/agile/1.0/board/456/sprint?startAt=50&maxResults=25", pages[2]);
    }
}