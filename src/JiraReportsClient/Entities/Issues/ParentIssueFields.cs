using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Jql;

    public class ParentIssueFields
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("priority")]
        public Priority Priority { get; set; }

        [JsonPropertyName("issuetype")]
        public IssueType IssueType { get; set; }
    }
