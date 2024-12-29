using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Json;
using JiraReportsClient.Utils.Json.Converters;

namespace JiraReportsClient.Entities.Projects;

[DebuggerDisplay("[{Key}] {Name}")]
public class Project
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(CustomStringToIntConverter))]
    public int Id { get; set; }

    [JsonPropertyName("key")] 
    public string Key { get; set; }

    [JsonPropertyName("name")] 
    public string Name { get; set; }
}