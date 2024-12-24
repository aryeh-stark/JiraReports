using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

public class StatFieldValue
{
    [JsonPropertyName("value")]
    public double? Value { get; set; }
}