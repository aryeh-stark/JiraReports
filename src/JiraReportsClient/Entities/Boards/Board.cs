using System.Text.Json;
using System.Text.Json.Serialization;

namespace JiraCmdLineTool.Common.Objects.Boards;

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

    public string TypeText
    {
        get
        {
            return Type.ToString();
        }
    }

    [JsonPropertyName("location")]
    public BoardLocation Location { get; set; }

    [JsonPropertyName("isPrivate")]
    public bool IsPrivate { get; set; }
}