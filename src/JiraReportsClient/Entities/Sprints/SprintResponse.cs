using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Sprints;

public class SprintResponse
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
    public required List<Sprint> Values { get; set; }
    
    public int Count => Values?.Count ?? 0;
    public bool HasValues() => Values?.Count != 0;
}