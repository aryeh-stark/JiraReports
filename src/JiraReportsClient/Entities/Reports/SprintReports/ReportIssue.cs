using System.Diagnostics;
using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient.Entities.Reports.SprintReports;

[DebuggerDisplay("[{Type}] {Key} {Summary} [{ReportType}]")]
public class ReportIssue(Issue issue, ReportIssueType reportType) : Issue(issue)
{
    public ReportIssueType ReportType { get; private set; } = reportType;

    public override string ToString() => $"[{ReportType}] [{Type}] {base.ToString()}";

    public ReportIssue AddTypeFlag(ReportIssueType additionalType)
    {
        ReportType |= additionalType;

        if (ReportType.HasFlag(ReportIssueType.Planned) && ReportType.HasFlag(ReportIssueType.Unplanned))
            ReportType &= ~ReportIssueType.Planned;

        return this;
    }

    public bool HasFlag(ReportIssueType flag) => (ReportType & flag) == flag;
}