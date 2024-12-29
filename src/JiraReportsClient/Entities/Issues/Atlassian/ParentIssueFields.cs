using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("[{IssueType}] {Summary}")]
public class ParentIssueFields
{
    [JsonPropertyName("summary")] 
    public string Summary { get; set; }

    [JsonPropertyName("status")] 
    public JiraStatus JiraStatus { get; set; }

    [JsonPropertyName("priority")]
    public Priority Priority { get; set; }

    [JsonPropertyName("issuetype")] 
    public IssueType IssueType { get; set; }
}