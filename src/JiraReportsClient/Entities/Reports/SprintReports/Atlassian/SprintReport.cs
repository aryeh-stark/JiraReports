using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

public class SprintReport
{
    [JsonPropertyName("contents")]
    public SprintReportContents Contents { get; set; }

    [JsonPropertyName("sprint")]
    public SprintDetail Sprint { get; set; }

    [JsonPropertyName("lastUserToClose")]
    public string LastUserToClose { get; set; }

    [JsonPropertyName("supportsPages")]
    public bool SupportsPages { get; set; }
}
