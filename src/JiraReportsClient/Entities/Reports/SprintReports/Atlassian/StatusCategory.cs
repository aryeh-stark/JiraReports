using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

public class StatusCategory
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("colorName")]
    public string ColorName { get; set; }
}