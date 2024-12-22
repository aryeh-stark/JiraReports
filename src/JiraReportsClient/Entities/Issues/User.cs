using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{DisplayName}")]
public class User
{
    [JsonPropertyName("accountId")]
    public string AccountId { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
}
