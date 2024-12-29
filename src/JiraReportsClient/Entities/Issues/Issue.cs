using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("[{Key}] [{Type}] {Summary}")]
public class Issue
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Key { get; set; }
    public string? Summary { get; set; }
    public IssueType Type { get; set; }
    public IssuePriority Priority { get; set; }
    public IssueStatus? Status { get; set; }
    public IssueStatusCategory? StatusCategory { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public User? Assignee { get; set; }
    public User? Creator { get; set; }
    public int? EstimationSeconds { get; set; }
    public EstimationTime Estimation => EstimationSeconds.HasValue ? new EstimationTime(EstimationSeconds.Value) : EstimationTime.Empty;
    public Team? Team { get; set; }
    public List<string> Labels { get; set; }
    public Parent? Parent { get; set; }
    public double? StoryPoints { get; set; }

    public Issue()
    {
    }
    
    public Issue(int id, string? key, string? summary, IssueType type, IssuePriority priority, IssueStatus? status, IssueStatusCategory? statusCategory, DateTime createdDate, DateTime updatedDate, User? assignee, User? creator, int? estimationSeconds, Team? team, List<string> labels, Parent? parent, double? storyPoints)
    {
        Id = id;
        Key = key;
        Summary = summary;
        Type = type;
        Priority = priority;
        Status = status;
        StatusCategory = statusCategory;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        Assignee = assignee;
        Creator = creator;
        EstimationSeconds = estimationSeconds;
        Team = team;
        Labels = labels;
        Parent = parent;
        StoryPoints = storyPoints;
    }
    
    public Issue(Issue issue)
    {
        Id = issue.Id;
        Key = issue.Key;
        Summary = issue.Summary;
        Type = issue.Type;
        Priority = issue.Priority;
        Status = issue.Status;
        StatusCategory = issue.StatusCategory;
        CreatedDate = issue.CreatedDate;
        UpdatedDate = issue.UpdatedDate;
        Assignee = issue.Assignee;
        Creator = issue.Creator;
        EstimationSeconds = issue.EstimationSeconds;
        Team = issue.Team;
        Labels = issue.Labels;
        Parent = issue.Parent;
        StoryPoints = issue.StoryPoints;
    }
}