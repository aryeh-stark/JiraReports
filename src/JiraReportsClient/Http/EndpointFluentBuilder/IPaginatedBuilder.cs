namespace JiraReportsClient.Http.Fluent;

public interface IPaginatedBuilder
{
    int StartAt { get; }
    int MaxResults { get; }
    string Build();

    int BuildCounter { get; }
}