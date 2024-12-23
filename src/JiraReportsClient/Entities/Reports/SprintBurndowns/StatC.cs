using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public class StatC
{
    [JsonPropertyName("newValue")]
    public double NewValue { get; set; }
}
