using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues.Atlassian.Json;
using JiraReportsClient.Entities.Projects;

namespace JiraReportsClient.Entities.Issues.Atlassian;

[DebuggerDisplay("[{IssueType}] {Summary}")]
public class IssueFields
{
    [JsonPropertyName("statuscategorychangedate")]
    [JsonConverter(typeof(JiraNullableDateTimeConverter))]
    public DateTime? StatusCategoryChangeDate { get; set; }

    [JsonPropertyName("parent")] 
    public ParentIssue Parent { get; set; }

    [JsonPropertyName("priority")] 
    public Priority Priority { get; set; }

    [JsonPropertyName("labels")] 
    public List<string> Labels { get; set; }

    [JsonPropertyName("timeestimate")] 
    public int? TimeEstimate { get; set; }

    [JsonPropertyName("aggregatetimeoriginalestimate")]
    public int? AggregateTimeOriginalEstimate { get; set; }

    [JsonPropertyName("assignee")] 
    public JiraUser Assignee { get; set; }

    [JsonPropertyName("status")] 
    public JiraStatus JiraStatus { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("aggregatetimeestimate")]
    public int? AggregateTimeEstimate { get; set; }

    [JsonPropertyName("creator")] 
    public JiraUser Creator { get; set; }

    [JsonPropertyName("reporter")] 
    public JiraUser Reporter { get; set; }

    [JsonPropertyName("issuetype")] 
    public IssueType IssueType { get; set; }

    [JsonPropertyName("project")] 
    public Project Project { get; set; }

    public List<JiraIssueSprint> RelatedSprints { get; set; }

    [JsonPropertyName("updated")] 
    [JsonConverter(typeof(JiraNullableDateTimeConverter))]
    public DateTime? Updated { get; set; }

    [JsonPropertyName("timeoriginalestimate")]
    public int? TimeOriginalEstimate { get; set; }

    [JsonPropertyName("summary")] 
    public string Summary { get; set; }

    public JiraTeam Team { get; set; }

    [JsonPropertyName("duedate")]
    [JsonConverter(typeof(JiraNullableDateTimeConverter))]
    public DateTime? DueDate { get; set; }
    
    public double? StoryPoints { get; set; }
}