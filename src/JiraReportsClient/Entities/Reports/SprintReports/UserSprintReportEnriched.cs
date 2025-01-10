using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient.Entities.Reports.SprintReports;

public class UserSprintReportEnriched(User user, SprintReportEnriched sprintReportEnriched) : SprintReportEnriched(
    reportIssue => reportIssue.Assignee?.Name == user.Name, sprintReportEnriched.Sprint,
    sprintReportEnriched.Issues, sprintReportEnriched.IssueChanges)
{
    public string Username => User.Name!;
    public User User { get; } = user ?? throw new ArgumentNullException(nameof(user));
}