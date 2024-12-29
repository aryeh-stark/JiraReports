using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues.Atlassian.Json;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("{Name}")]
public class JiraStatus
{
    [JsonPropertyName("name")] 
    public string Name { get; set; }

    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("description")] 
    public string Description { get; set; }

    [JsonPropertyName("statusCategory")] 
    public JiraStatusCategory JiraStatusCategory { get; set; }
}