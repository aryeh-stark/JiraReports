using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Boards.Atlassian;

[DebuggerDisplay("{Type} Board: {Name} ({Id})")]
public class JiraBoard
{   
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("self")]
    public string Self { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BoardTypes Type { get; set; }

    [JsonPropertyName("location")]
    public JiraBoardLocation Location { get; set; }

    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; set; }
}