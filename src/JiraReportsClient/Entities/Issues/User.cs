using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues.Json;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{Name}")]
[JsonConverter(typeof(UserJsonConverter))]
public class User
{
    public User(string? name, string? id, string? email)
    {
        Name = name;
        Id = id;
        Email = email;
    }

    public string? Name { get; set; }
    //[JsonIgnore]
    public string? Id { get; set; }
    //[JsonIgnore]
    public string? Email { get; set; }
}