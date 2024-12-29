using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues.Atlassian.Json;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("{DisplayName}")]
public class JiraUser
{
    [JsonPropertyName("accountId")]
    public string AccountId { get; set; }

    [JsonPropertyName("emailAddress")]
    public string? EmailAddress { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }
}
