using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public class Column
{
    [JsonPropertyName("notDone")]
    public bool? NotDone { get; set; }

    [JsonPropertyName("done")]
    public bool? Done { get; set; }

    [JsonPropertyName("newStatus")]
    public string NewStatus { get; set; }
}
