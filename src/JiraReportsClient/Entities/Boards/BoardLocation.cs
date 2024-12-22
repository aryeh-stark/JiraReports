using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Boards;

public class BoardLocation
{
    [JsonPropertyName("projectId")]
    public int? ProjectId { get; set; }

    [JsonPropertyName("userId")]
    public int? UserId { get; set; }

    [JsonPropertyName("userAccountId")]
    public string UserAccountId { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }

    [JsonPropertyName("projectName")]
    public string ProjectName { get; set; }

    [JsonPropertyName("projectKey")]
    public string ProjectKey { get; set; }

    [JsonPropertyName("projectTypeKey")]
    public string ProjectTypeKey { get; set; }

    [JsonPropertyName("avatarURI")]
    public string AvatarUri { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}