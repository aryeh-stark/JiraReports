namespace JiraReportsClient.Entities.Issues;

public enum IssueChangeType
{
    Undefined = -1,
    AddedToSprint = 1,
    RemovedFromSprint,
    BurndownEvent
}