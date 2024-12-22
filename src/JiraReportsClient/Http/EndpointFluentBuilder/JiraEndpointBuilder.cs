namespace JiraReportsClient.Http.EndpointFluentBuilder;

public class JiraEndpointBuilder(string baseUrl, int maxResults)
{
    public BoardsBuilder Boards() => new(baseUrl, maxResults);
    public SprintBuilder Sprints() => new(baseUrl, maxResults);
    public ProjectsBuilder Projects() => new(baseUrl, maxResults);
    public IssuesBuilder Issues() => new(baseUrl, maxResults);
    public ReportsBuilder Reports() => new(baseUrl, maxResults);
}