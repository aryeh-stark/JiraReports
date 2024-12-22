using System.Text.Json.Serialization;

namespace JiraCmdLineTool.Common.Objects.SprintReports;

public class IssueStatus
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("iconUrl")]
    public string IconUrl { get; set; }

    [JsonPropertyName("statusCategory")]
    public StatusCategory StatusCategory { get; set; }
}