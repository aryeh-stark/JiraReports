namespace JiraReportsClient.Http.Fluent;

public class IssuesBuilder(string baseUrl, int maxResults) 
    : JiraEndpointBuilderBase<IssuesBuilder>(baseUrl, maxResults), IPaginatedBuilder
{
    private List<string> _issueIds = new();

    public IssuesBuilder ForIssues(IEnumerable<string> issueIds)
    {
        _issueIds = issueIds.ToList();
        return this;
    }

    public override string Build()
    {
        if (!_issueIds.Any())
            throw new InvalidOperationException("At least one issue ID is required");

        IncrementBuildCounter();
        
        var jql = $"issue in ({string.Join(",", _issueIds)}) order by key";
        return $"{BaseUrl}/rest/api/3/search?jql={jql}";
    }
}