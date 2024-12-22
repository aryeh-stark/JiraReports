using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Jql;

[DebuggerDisplay("{Key} - {Fields.IssueType.Name}")]
public class JiraIssue
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("fields")]
        public IssueFields Fields { get; set; }
    }
