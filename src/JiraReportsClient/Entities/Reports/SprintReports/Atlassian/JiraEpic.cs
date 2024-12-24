using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

public class JiraEpic
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("editable")]
    public bool Editable { get; set; }

    [JsonPropertyName("renderer")]
    public string Renderer { get; set; }

    [JsonPropertyName("issueId")]
    public int IssueId { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("epicKey")]
    public string EpicKey { get; set; }

    [JsonPropertyName("epicColor")]
    public string EpicColor { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("issueTypeId")]
    public string IssueTypeId { get; set; }

    [JsonPropertyName("issueTypeIconUrl")]
    public string IssueTypeIconUrl { get; set; }

    [JsonPropertyName("canRemoveEpic")]
    public bool CanRemoveEpic { get; set; }
}