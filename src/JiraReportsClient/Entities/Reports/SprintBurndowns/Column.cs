using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Json;
using JiraReportsClient.Utils.Json.Converters;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public class Column
{
    [JsonPropertyName("notDone")]
    public bool? NotDone { get; set; }

    [JsonPropertyName("done")]
    public bool? Done { get; set; }

    [JsonPropertyName("newStatus")]
    [JsonConverter(typeof(CustomStringToNullableIntConverter))]
    public int? NewStatus { get; set; }
}
