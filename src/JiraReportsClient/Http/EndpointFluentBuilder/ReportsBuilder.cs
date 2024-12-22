namespace JiraReportsClient.Http.EndpointFluentBuilder;

public class ReportsBuilder(string baseUrl, int maxResults) 
    : JiraEndpointBuilderBase<ReportsBuilder>(baseUrl, maxResults), IPaginatedBuilder
{
    private int? _boardId;
    private int? _sprintId;

    public ReportsBuilder ForBoard(int boardId)
    {
        _boardId = boardId;
        return this;
    }

    public ReportsBuilder ForSprint(int sprintId)
    {
        _sprintId = sprintId;
        return this;
    }

    public string BuildSprintReport()
    {
        ValidateIds();
        return $"{BaseUrl}/rest/greenhopper/1.0/rapid/charts/sprintreport?rapidViewId={_boardId}&sprintId={_sprintId}";
    }

    public string BuildBurndownReport()
    {
        ValidateIds();
        return $"{BaseUrl}/rest/greenhopper/1.0/rapid/charts/scopechangeburndownchart.json?rapidViewId={_boardId}&sprintId={_sprintId}&statisticFieldId=issueCount_";
    }

    public override string Build()
    {
        IncrementBuildCounter();
        
        return BuildSprintReport();
    }

    private void ValidateIds()
    {
        if (!_boardId.HasValue)
            throw new InvalidOperationException("Board ID is required");
        if (!_sprintId.HasValue)
            throw new InvalidOperationException("Sprint ID is required");
    }
}