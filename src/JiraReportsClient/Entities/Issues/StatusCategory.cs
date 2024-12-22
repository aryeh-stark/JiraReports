using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{Name}")]
public class StatusCategory
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