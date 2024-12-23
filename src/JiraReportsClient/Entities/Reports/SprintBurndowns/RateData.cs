using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public class RateData
{
    [JsonPropertyName("start")]
    public long Start { get; set; }

    [JsonPropertyName("end")]
    public long End { get; set; }

    [JsonPropertyName("rate")]
    public int Rate { get; set; }
}