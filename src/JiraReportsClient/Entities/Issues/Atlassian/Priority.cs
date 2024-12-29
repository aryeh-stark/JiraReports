using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Json.Converters;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("{Level}")]
public class Priority
{
    [JsonPropertyName("name")] 
    public string Level { get; set; }

    [JsonPropertyName("id")] 
    public string Id { get; set; }
}