using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

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