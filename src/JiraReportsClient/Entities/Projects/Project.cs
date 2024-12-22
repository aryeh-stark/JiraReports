using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Projects;

[DebuggerDisplay("Project: {Name} (Key: {Key}, Id: {Id})")]
public class Project
{
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("key")] 
    public string Key { get; set; }

    [JsonPropertyName("name")] 
    public string Name { get; set; }
}