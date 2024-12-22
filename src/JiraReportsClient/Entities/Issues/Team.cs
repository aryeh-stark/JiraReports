using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{Name}")]
public class Team
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("avatarUrl")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("isVisible")]
    public bool IsVisible { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("isShared")]
    public bool IsShared { get; set; }
}
