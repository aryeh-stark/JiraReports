using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("[{Name}] [{StartDate} -> {EndDate}] [{State}]")]
public class JiraIssueSprint
{
    [JsonPropertyName("id")] 
    public int Id { get; set; }
    [JsonPropertyName("name")] 
    public string Name { get; set; }
    [JsonPropertyName("state")] 
    public SprintState State { get; set; }
    [JsonPropertyName("boardId")] 
    public int BoardId { get; set; }
    [JsonPropertyName("goal")] 
    public string Goal { get; set; }
    [JsonPropertyName("startDate")] 
    public DateTime StartDate { get; set; }
    [JsonPropertyName("endDate")] 
    public DateTime EndDate { get; set; }
    [JsonPropertyName("completeDate")] 
    public DateTime CompleteDate { get; set; }
}