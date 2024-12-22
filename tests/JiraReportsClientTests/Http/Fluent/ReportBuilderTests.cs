using JiraReportsClient.Http.EndpointFluentBuilder;

namespace JiraReportsClientTests.Http.Fluent;

public class ReportBuilderTests
{
    private const string BaseUrl = "https://jira.example.com";
    private readonly JiraEndpointBuilder _builder = new(BaseUrl, 50);

    [Fact]
    public void Build_WithoutBoardId_ThrowsException()
    {
        var builder = _builder.Reports();
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Board ID is required", exception.Message);
    }

    [Fact]
    public void Build_WithoutSprintId_ThrowsException()
    {
        var builder = _builder.Reports().ForBoard(123);
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Sprint ID is required", exception.Message);
    }

    [Fact]
    public void BuildSprintReport_ReturnsCorrectUrl()
    {
        var url = _builder.Reports()
            .ForBoard(123)
            .ForSprint(456)
            .BuildSprintReport();

        Assert.Equal($"{BaseUrl}/rest/greenhopper/1.0/rapid/charts/sprintreport?rapidViewId=123&sprintId=456", url);
    }

    [Fact]
    public void BuildBurndownReport_ReturnsCorrectUrl()
    {
        var url = _builder.Reports()
            .ForBoard(123)
            .ForSprint(456)
            .BuildBurndownReport();

        Assert.Equal($"{BaseUrl}/rest/greenhopper/1.0/rapid/charts/scopechangeburndownchart.json?rapidViewId=123&sprintId=456&statisticFieldId=issueCount_", url);
    }

    [Fact]
    public void Build_CallsSprintReport()
    {
        var sprintReport = _builder.Reports()
            .ForBoard(123)
            .ForSprint(456)
            .Build();
        
        var expectedSprintReport = _builder.Reports()
            .ForBoard(123)
            .ForSprint(456)
            .BuildSprintReport();

        Assert.Equal(expectedSprintReport, sprintReport);
    }
}