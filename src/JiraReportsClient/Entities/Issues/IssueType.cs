namespace JiraReportsClient.Entities.Issues;

public enum IssueType
{
    Undefined = -1,
    Epic = 1,
    Story,
    Task,
    Bug,
    SubTask,
    CSBug,
}