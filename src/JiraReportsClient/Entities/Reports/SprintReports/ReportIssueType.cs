namespace JiraReportsClient.Entities.Reports.SprintReports;

[Flags]
public enum ReportIssueType
{
    Unspecified = 0,
    Planned = 1 << 0,
    Unplanned = 1 << 1,
    Cancelled = 1 << 2,
    Done = 1 << 3,
    NotDone = 1 << 4,
    Removed = 1 << 5,
}