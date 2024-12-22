using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay($"[{{Key}}] [{{Fields.IssueType.Name}}] {{Fields.Summary}}")]
public class JiraIssue
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("fields")]
        public IssueFields Fields { get; set; }
    }
