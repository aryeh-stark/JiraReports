using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues.Atlassian.Json;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("{Name}")]
public class IssueType
{
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("name")] 
    public string Name { get; set; }

    [JsonPropertyName("hierarchyLevel")] 
    public int? HierarchyLevel { get; set; }
}