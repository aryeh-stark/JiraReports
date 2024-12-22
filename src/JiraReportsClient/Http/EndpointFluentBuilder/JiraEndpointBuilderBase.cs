namespace JiraReportsClient.Http.EndpointFluentBuilder;

public abstract class JiraEndpointBuilderBase<TSelf>(string baseUrl, int maxResults) 
    : IPaginatedBuilder 
        where TSelf : JiraEndpointBuilderBase<TSelf>, IPaginatedBuilder
{
    private int _startAt;
    private int _maxResults = maxResults;
    private int _buildCounter = 0;
        
    public virtual int StartAt => _startAt;
    public virtual int MaxResults => _maxResults;
    
    public virtual int BuildCounter => _buildCounter;
    protected string BaseUrl => baseUrl;
        
    public abstract string Build();

    public TSelf WithPagination(int startAt, int maxResultsForPage)
    {
        _startAt = startAt;
        _maxResults = maxResultsForPage;
        return (TSelf)this;
    }
     
    public TSelf WithPagination(int maxResultsForPage)
    {
        _maxResults = maxResultsForPage;
        return (TSelf)this;
    }
    
    public TSelf WithPagination()
    {
        _startAt = 0;
        _maxResults = MaxResults;
        return (TSelf)this;
    }
    
    protected void IncrementBuildCounter()
    {
        _buildCounter++;
    }
}