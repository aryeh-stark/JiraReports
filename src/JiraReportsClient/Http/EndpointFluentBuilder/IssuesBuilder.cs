namespace JiraReportsClient.Http.EndpointFluentBuilder;

public class IssuesBuilder : JiraEndpointBuilderBase<IssuesBuilder>, IPaginatedBuilder
{
    private List<string> _issueIds;
    private int? _sprintId;

    public IssuesBuilder(string baseUrl, int maxResults) : base(baseUrl, maxResults)
    {
        _issueIds = new List<string>();
        _sprintId = null;
    }

    private IssuesBuilder(int? sprintId, List<string> issueIds, string baseUrl, int maxResults) : base(baseUrl, maxResults)
    {
        _issueIds = issueIds;
        _sprintId = sprintId;
    }

    public IssuesBuilder ForIssues(IEnumerable<string> issueIds)
    {
        return new IssuesBuilder(_sprintId, issueIds.ToList(), BaseUrl, MaxResults);
    }
    
    public IssuesBuilder ForSprint(int sprintId)
    {
        return new IssuesBuilder(sprintId, _issueIds, BaseUrl, MaxResults);
    }

    public override string Build()
    {
        try
        {
            if (_sprintId.HasValue && _sprintId.Value > 0)
                return $"{BaseUrl}/rest/agile/1.0/sprint/{_sprintId.Value}/issue";

            if (!_issueIds.Any())
                throw new InvalidOperationException("At least one issue ID is required");

            var jql = $"issue in ({string.Join(",", _issueIds)}) order by key";
            return $"{BaseUrl}/rest/api/3/search?jql={jql}";
        }
        finally
        {
            IncrementBuildCounter();
        }
    }
}