using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

public class SprintReportResponse
{
    [JsonPropertyName("contents")]
    public SprintReportContents? Contents { get; set; }

    [JsonPropertyName("sprint")]
    public SprintDetail Sprint { get; set; }

    [JsonPropertyName("lastUserToClose")]
    public string LastUserToClose { get; set; }

    [JsonPropertyName("supportsPages")]
    public bool SupportsPages { get; set; }
    
    public bool HasIssues()
    {
        return Contents != null && Contents.HasIssues();
    }
}
