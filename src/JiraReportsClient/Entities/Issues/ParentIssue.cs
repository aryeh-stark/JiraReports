using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Jql;

    public class ParentIssue
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("fields")]
        public ParentIssueFields Fields { get; set; }
    }
