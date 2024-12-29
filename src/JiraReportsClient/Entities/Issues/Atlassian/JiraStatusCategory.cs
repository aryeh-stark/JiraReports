using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues.Atlassian.Json;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("{Name}")]
public class JiraStatusCategory
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("colorName")]
    public string ColorName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}