using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Boards.Atlassian;

public class JiraBoardsResponse
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
    public required List<JiraBoard> Values { get; set; }

    public bool HasValues() => Values?.Count > 0;
    
    public int Count => Values?.Count ?? 0;
}