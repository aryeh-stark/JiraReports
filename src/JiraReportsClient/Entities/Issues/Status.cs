using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{Name}")]
public class Status
{
    [JsonPropertyName("name")] 
    public string Name { get; set; }

    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("description")] 
    public string Description { get; set; }

    [JsonPropertyName("statusCategory")] 
    public StatusCategory StatusCategory { get; set; }
}