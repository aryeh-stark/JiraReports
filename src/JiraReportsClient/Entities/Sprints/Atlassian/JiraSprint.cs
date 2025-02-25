using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Boards.Atlassian;

namespace JiraReportsClient.Entities.Sprints.Atlassian;

[DebuggerDisplay("Sprint: {Name} ({Id})")]
public class JiraSprint
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("self")]
    public string Self { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("completeDate")]
    public DateTime? CompleteDate { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("originBoardId")]
    public int OriginBoardId { get; set; }

    [JsonPropertyName("goal")]
    public string Goal { get; set; }

    public JiraBoard JiraBoard { get; set; }
}