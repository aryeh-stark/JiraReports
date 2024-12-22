namespace JiraReportsClient.Http.EndpointFluentBuilder;

public class BoardsBuilder(string baseUrl, int maxResults)
    : JiraEndpointBuilderBase<BoardsBuilder>(baseUrl, maxResults), IPaginatedBuilder
{
    private string? _projectKey;
    private int _boardId;

    public BoardsBuilder ForProject(string projectKey)
    {
        _projectKey = projectKey;
        return this;
    }
    
    public BoardsBuilder ById(int boardId)
    {
        _boardId = boardId;
        return this;
    }

    public override string Build()
    {
        try
        {
            if (_boardId > 0)
                return $"{BaseUrl}/rest/agile/1.0/board/{_boardId}";
        
            if (!string.IsNullOrEmpty(_projectKey))
                return $"{BaseUrl}/rest/agile/1.0/board?projectKeyOrId={_projectKey}&startAt={StartAt}&maxResults={MaxResults}";
            
            return $"{BaseUrl}/rest/agile/1.0/board?startAt={StartAt}&maxResults={MaxResults}";
        }
        finally
        {
            IncrementBuildCounter();
        }
    }
}