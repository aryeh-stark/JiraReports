using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Json;
using JiraReportsClient.Utils.Json.Converters;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("[{Key}] [{Fields.IssueType.Name}] {Fields.Summary}")]
public class JiraIssue
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(CustomStringToIntConverter))]
        public int Id { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("fields")]
        public IssueFields Fields { get; set; }
    }
