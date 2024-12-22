using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Jql;

public class Priority
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
