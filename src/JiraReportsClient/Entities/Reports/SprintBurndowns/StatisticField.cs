using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public class StatisticField
{
    [JsonPropertyName("typeId")]
    public string TypeId { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("isValid")]
    public bool IsValid { get; set; }

    [JsonPropertyName("isEnabled")]
    public bool IsEnabled { get; set; }

    [JsonPropertyName("renderer")]
    public string Renderer { get; set; }
}
