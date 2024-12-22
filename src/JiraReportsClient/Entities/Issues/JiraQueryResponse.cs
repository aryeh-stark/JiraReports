using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Jql;

public class JiraQueryResponse
    {
        [JsonPropertyName("expand")]
        public string Expand { get; set; }

        [JsonPropertyName("startAt")]
        public int StartAt { get; set; }

        [JsonPropertyName("maxResults")]
        public int MaxResults { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("issues")]
        public List<JiraIssue> Issues { get; set; }
    }
