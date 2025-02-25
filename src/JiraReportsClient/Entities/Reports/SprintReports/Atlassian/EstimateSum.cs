using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

public class EstimateSum
{
    [JsonPropertyName("value")]
    public double? Value { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}