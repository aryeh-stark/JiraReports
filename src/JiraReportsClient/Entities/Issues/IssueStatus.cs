namespace JiraReportsClient.Entities.Issues;

public enum IssueStatus
{
    Undefined = -1,
    Open = 1,
    ReadyForWork,
    InProgress,
    InReview,
    Blocked,
    BlockedInternal,
    OnHoldExternal,
    Done,
    Merged,
    Cancelled
}