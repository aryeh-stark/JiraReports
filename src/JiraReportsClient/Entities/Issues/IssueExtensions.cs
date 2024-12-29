using System.Text.RegularExpressions;
using JiraReportsClient.Entities.Issues.Atlassian;

namespace JiraReportsClient.Entities.Issues;

public static class IssueExtensions
{
    private static string RemoveSpecialCharacters(this string str)
    {
        return Regex.Replace(str, @"[()\s]", "", RegexOptions.Compiled);
    }
    
    public static IssueType ToModel(this Entities.Issues.Atlassian.IssueType issueType)
    {
        try
        {
            return Enum.Parse<IssueType>(issueType.Name.RemoveSpecialCharacters(), true);
        }
        catch
        {
            return IssueType.Undefined;
        }
    }


    public static IssuePriority ToModel(this Priority priority)
    {
        try
        {
            return Enum.Parse<IssuePriority>(priority.Level.RemoveSpecialCharacters(), true);
        }
        catch
        {
            return IssuePriority.Undefined;
        }
    }

    public static IssueStatus ToModel(this JiraStatus jiraStatus)
    {
        try
        {
            return Enum.Parse<IssueStatus>(jiraStatus.Name.RemoveSpecialCharacters(), true);
        }
        catch
        {
            return IssueStatus.Undefined;
        }
    }

    public static IssueStatusCategory ToModel(this JiraStatusCategory jiraStatusCategory)
    {
        try
        {
            return Enum.Parse<IssueStatusCategory>(jiraStatusCategory.Name.RemoveSpecialCharacters(), true);
        }
        catch
        {
            return IssueStatusCategory.Undefined;
        }
    }

    public static Parent? ToModel(this ParentIssue? parent)
    {
        if (parent == null)
        {
            return null;
        }
        
        return new Parent
        {
            Id = parent.Id,
            Key = parent.Key,
            Summary = parent.Fields.Summary,
            Status = parent.Fields.JiraStatus.ToModel(),
            Priority = parent.Fields.Priority.ToModel(),
            Type = parent.Fields.IssueType.ToModel()
        };
    }

    public static User ToUser(this JiraUser jiraUser)
    {
        return new User(jiraUser.DisplayName, jiraUser.AccountId, jiraUser.EmailAddress);
    }
    
    public static User? GetAssignee(this IssueFields fields)
    {
        return fields is { Assignee: null } ? null : fields.Assignee.ToUser();
    }
    
    public static User? GetCreator(this IssueFields fields)
    {
        return fields is { Creator: null } ? null : fields.Creator.ToUser();
    }
    
    public static Parent? GetParent(this IssueFields fields)
    {
        return fields is { Parent: null } ? null : fields.Parent.ToModel();
    }
    
    private static Team ToModel(this JiraTeam jiraTeam)
    {
        return new Team(jiraTeam.Name, jiraTeam.Id);
    }
    
    public static Team? GetTeam(this IssueFields fields)
    {
        return fields is { Team: null } ? null : fields.Team.ToModel();
    }
    
    public static IssueStatus? GetStatus(this IssueFields fields)
    {
        return fields is { JiraStatus: null } ? null : fields.JiraStatus.ToModel();
    }
    
    public static IssueStatusCategory? GetStatusCategory(this IssueFields fields)
    {
        return fields is { JiraStatus: null } ? null : fields.JiraStatus.JiraStatusCategory.ToModel();
    }
    
    public static bool IsCancelled(this Issue issue)
    {
        return issue.Status == IssueStatus.Cancelled;
    }

    public static Issue ToModel(this JiraIssue jiraIssue)
    {
        try
        {
            return new Issue
            {
                Id = jiraIssue.Id,
                Key = jiraIssue.Key,
                Summary = jiraIssue.Fields.Summary,
                Type = jiraIssue.Fields.IssueType.ToModel(),
                Priority = jiraIssue.Fields.Priority.ToModel(),
                Status = jiraIssue.Fields.GetStatus(),
                StatusCategory = jiraIssue.Fields.GetStatusCategory(),
                CreatedDate = Convert.ToDateTime(jiraIssue.Fields.Created),
                UpdatedDate = Convert.ToDateTime(jiraIssue.Fields.Updated),
                Assignee = jiraIssue.Fields.GetAssignee(),
                Creator = jiraIssue.Fields.GetCreator(),
                EstimationSeconds = jiraIssue.Fields.TimeOriginalEstimate,
                Team = jiraIssue.Fields.GetTeam(),
                Labels = jiraIssue.Fields.Labels,
                Parent = jiraIssue.Fields.GetParent(),
                StoryPoints = jiraIssue.Fields.StoryPoints
            };
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    public static IReadOnlyList<Issue> ToModelReadOnlyList(this IEnumerable<JiraIssue> jiraIssues)
    {
        return jiraIssues.Select(s => s.ToModel()).ToList();
    }
}