using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Sprints.Atlassian;

public class JiraSprintResponse
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
    public required List<JiraSprint> Values { get; set; }
    
    public int Count => Values?.Count ?? 0;
    public bool HasValues() => Values?.Count != 0;
}