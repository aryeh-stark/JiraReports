namespace JiraReportsClient.Http.Fluent;

public class ProjectsBuilder(string baseUrl, int maxResults)
    : JiraEndpointBuilderBase<ProjectsBuilder>(baseUrl, maxResults), IPaginatedBuilder
{
    public override string Build()
    {
        IncrementBuildCounter();
        return $"{BaseUrl}/rest/api/3/project/search?startAt={StartAt}&maxResults={MaxResults}&orderBy=name";
    }
}