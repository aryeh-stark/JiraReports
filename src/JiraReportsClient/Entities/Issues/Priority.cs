using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{Name}")]
public class Priority
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
