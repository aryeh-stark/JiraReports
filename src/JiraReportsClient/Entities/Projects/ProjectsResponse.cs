using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Projects;

public class ProjectsResponse
{
    [JsonPropertyName("maxResults")]
    public int MaxResults { get; set; }

    [JsonPropertyName("startAt")]
    public int StartAt { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("isLast")]
    public bool IsLast { get; set; }

    [JsonPropertyName("values")]
    public required List<Project> Values { get; set; }

    public bool HasValues() => Values?.Count > 0;
}