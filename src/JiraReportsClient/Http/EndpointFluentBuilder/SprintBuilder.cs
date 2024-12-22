namespace JiraReportsClient.Http.Fluent;

public class SprintBuilder(string baseUrl, int maxResults) 
    : JiraEndpointBuilderBase<SprintBuilder>(baseUrl, maxResults), IPaginatedBuilder
{
    private int? _sprintId;
    private int? _boardId;
    private bool _includeFutureSprints;
    private bool _includeActiveSprints;
    private bool _includeClosedSprints;

    public SprintBuilder ForSprint(int sprintId)
    {
        _sprintId = sprintId;
        return this;
    }

    public SprintBuilder ForBoard(int boardId)
    {
        _boardId = boardId;
        return this;
    }

    public SprintBuilder IncludeStates(bool future = false, bool active = false, bool closed = false)
    {
        _includeFutureSprints = future;
        _includeActiveSprints = active;
        _includeClosedSprints = closed;
        return this;
    }

    public override string Build()
    {
        IncrementBuildCounter();
        
        if (_sprintId.HasValue)
            return $"{BaseUrl}/rest/agile/1.0/sprint/{_sprintId}";

        if (!_boardId.HasValue)
            throw new InvalidOperationException("Board ID is required");

        var state = new List<string>
        {
            _includeFutureSprints ? "future" : "",
            _includeActiveSprints ? "active" : "",
            _includeClosedSprints ? "closed" : ""
        }.Where(x => !string.IsNullOrEmpty(x)).ToList();
            
        var stateQuery = state.Count > 0 ? "&state=" + string.Join(",", state) : "";
        return $"{BaseUrl}/rest/agile/1.0/board/{_boardId}/sprint?startAt={StartAt}&maxResults={MaxResults}{stateQuery}";
    }
}