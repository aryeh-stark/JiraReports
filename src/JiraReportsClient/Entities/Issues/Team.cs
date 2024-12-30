using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues.Json;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{Name}")]
[JsonConverter(typeof(TeamJsonConverter))]
public class Team(string? id, string? name)
{
    //[JsonIgnore]
    public string? Id { get; set; } = id;
    public string? Name { get; set; } = name;
}