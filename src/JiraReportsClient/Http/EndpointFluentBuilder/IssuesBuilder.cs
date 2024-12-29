namespace JiraReportsClient.Http.EndpointFluentBuilder;

public class IssuesBuilder : JiraEndpointBuilderBase<IssuesBuilder>, IPaginatedBuilder
{
    private List<string> _issueIds;
    private int? _sprintId;
    private QueryType _queryType;

    public enum QueryType
    {
        Single,
        Search
    }

    public IssuesBuilder(string baseUrl, int maxResults, QueryType queryType = QueryType.Search) : base(baseUrl, maxResults)
    {
        _issueIds = new List<string>();
        _sprintId = null;
        _queryType = queryType;
    }

    private IssuesBuilder(int? sprintId, List<string> issueIds, string baseUrl, int maxResults, QueryType queryType = QueryType.Search) : base(baseUrl,
        maxResults)
    {
        _issueIds = issueIds;
        _sprintId = sprintId;
        _queryType = queryType;
    }

    public IssuesBuilder ForIssue(string issueId)
    {
        return new IssuesBuilder(_sprintId, [issueId], BaseUrl, MaxResults, QueryType.Single);
    }

    public IssuesBuilder ForIssues(IEnumerable<string> issueIds)
    {
        return new IssuesBuilder(_sprintId, issueIds.ToList(), BaseUrl, MaxResults, QueryType.Search);
    }

    public IssuesBuilder ForSprint(int sprintId)
    {
        return new IssuesBuilder(sprintId, _issueIds, BaseUrl, MaxResults, QueryType.Search);
    }

    public override string Build()
    {
        try
        {
            if (_queryType == QueryType.Single && _issueIds.Any() && _issueIds.Count == 1)
                return $"{BaseUrl}/rest/api/3/issue/{_issueIds.First()}";
            
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