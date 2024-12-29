using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Json;
using JiraReportsClient.Utils.Json.Converters;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("[{Key}] {Fields.Summary}")]
public class ParentIssue
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(CustomStringToIntConverter))]
    public int Id { get; set; }

    [JsonPropertyName("key")] 
    public string Key { get; set; }

    [JsonPropertyName("fields")] 
    public ParentIssueFields Fields { get; set; }
}