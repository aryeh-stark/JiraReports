using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Boards;

[DebuggerDisplay("{Type} Board: {Name} ({Id})")]
public class Board
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
    public BoardLocation Location { get; set; }

    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; set; }
}