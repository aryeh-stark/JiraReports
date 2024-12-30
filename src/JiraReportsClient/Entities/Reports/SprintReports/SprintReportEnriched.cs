using System.Collections.ObjectModel;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Reports.SprintBurndowns;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient.Entities.Reports.SprintReports;

public class SprintReportEnriched
{
   public Board Board => Sprint.Board;
    public Sprint Sprint { get; }
    public IReadOnlyList<ReportIssue> Issues { get; }

    public IReadOnlyList<ReportIssue> CompletedIssues { get;  }

    public IReadOnlyList<ReportIssue> IncompleteIssues { get;  }

    public IReadOnlyList<ReportIssue> RemovedIssues { get;  }
    public IReadOnlyList<ReportIssue> CancelledIssues { get;  }

    public IReadOnlyList<ReportIssue> PlannedIssues { get;  }
    public IReadOnlyList<ReportIssue> AddedAfterSprintStart { get;  }

    public IReadOnlyList<ReportIssue> RemovedAfterSprintStart { get;  }

    public IReadOnlyList<ReportIssue> AddedAndRemovedAfterSprintStart { get;  }

    public IReadOnlyDictionary<string, IReadOnlyList<IssueChange>> IssueChanges { get;  }

    public SprintMetrics Metrics { get; }

    public SprintReportEnriched(
        Sprint sprint,
        IEnumerable<Issue>? issues,
        IEnumerable<Issue>? completedIssues,
        IEnumerable<Issue>? incompleteIssues,
        IEnumerable<Issue>? removedIssues,
        IReadOnlyDictionary<string, IReadOnlyList<IssueChange>>? issueChanges)
    {
        var reportIssuesLocalCache = issues?
            .ToDictionary(i => i.Key, i => new ReportIssue(i, ReportIssueType.Unspecified)) ?? new Dictionary<string, ReportIssue>()!;
        var userByName = new Dictionary<string, User>();
        Sprint = sprint;
        CompletedIssues = completedIssues != null ? EnrichIssues(completedIssues, ReportIssueType.Done) : [];
        IncompleteIssues = incompleteIssues != null ? EnrichIssues(incompleteIssues, ReportIssueType.NotDone) : [];
        RemovedIssues = removedIssues != null ? EnrichIssues(removedIssues, ReportIssueType.Removed) : [];
        CancelledIssues = issues.Where(i => i.IsCancelled()).Select(i => EnrichIssue(i, ReportIssueType.Cancelled)).ToList();
        IssueChanges = issueChanges ?? new Dictionary<string, IReadOnlyList<IssueChange>>();

        AddedAfterSprintStart = issues
            .Where(i => i.Key != null && IssueChanges.ContainsKey(i.Key))
            .Where(i => i.Key != null && IssueChanges[i.Key].Any(x => x.ChangeType == IssueChangeType.AddedToSprint))
            .Select(i => EnrichIssue(i, ReportIssueType.Unplanned))
            .ToList();
        PlannedIssues = issues
            .Except(AddedAfterSprintStart)
            .Select(i => EnrichIssue(i, ReportIssueType.Planned))
            .ToList();
        RemovedAfterSprintStart = issues
            .Where(i => i.Key != null && IssueChanges.ContainsKey(i.Key))
            .Where(i => i.Key != null && IssueChanges[i.Key].Any(x => x.ChangeType == IssueChangeType.RemovedFromSprint))
            .Select(i => EnrichIssue(i, ReportIssueType.Removed))
            .ToList();
        AddedAndRemovedAfterSprintStart = issues
            .Where(i => i.Key != null && IssueChanges.ContainsKey(i.Key))
            .Where(i => i.Key != null &&
                        IssueChanges[i.Key].Any(x => x.ChangeType == IssueChangeType.AddedToSprint) &&
                        IssueChanges[i.Key].Any(x => x.ChangeType == IssueChangeType.RemovedFromSprint))
            .Select(i => EnrichIssue(i, ReportIssueType.Unplanned | ReportIssueType.Removed))
            .ToList();
        
        Issues = reportIssuesLocalCache.Values.ToList();
        Metrics = new SprintMetrics(sprint, Issues);
        
        ReportIssue EnrichIssue(Issue issue, ReportIssueType type) 
        {
            if (issue.Assignee == null)
            {
                if (!userByName.TryGetValue(issue.Assignee?.Name ?? "", out var user))
                {
                    userByName[user.Name] = user;
                }
                else
                {
                    issue.Assignee = user;
                }
            }

            if (!reportIssuesLocalCache.TryGetValue(issue.Key, out var reportIssue))
            {
                reportIssue = new ReportIssue(issue, type);
                reportIssuesLocalCache[issue.Key] = reportIssue;
            }
            else
            {
                reportIssuesLocalCache[issue.Key] = reportIssue.AddTypeFlag(type);   
            }
            
            return reportIssuesLocalCache[issue.Key];
        }
        
        IReadOnlyList<ReportIssue> EnrichIssues(IEnumerable<Issue> issuesEnumerable, ReportIssueType type) 
        {
            return issuesEnumerable.Select(i => EnrichIssue(i, type)).ToList();
        }
    }
    
    protected SprintReportEnriched(
        Func<ReportIssue, bool> predicate,
        Sprint sprint, 
        IReadOnlyList<ReportIssue> issues, 
        IReadOnlyDictionary<string, IReadOnlyList<IssueChange>> issueChanges)
    {
        Sprint = sprint;
        Issues = issues.Where(predicate).ToList();
        CompletedIssues = Issues.Where(i => i.HasFlag(ReportIssueType.Done)).ToList();
        IncompleteIssues = Issues.Where(i => i.HasFlag(ReportIssueType.NotDone)).ToList();
        RemovedIssues = Issues.Where(i => i.HasFlag(ReportIssueType.Removed)).ToList();
        CancelledIssues = Issues.Where(i => i.HasFlag(ReportIssueType.Cancelled)).ToList();
        PlannedIssues = Issues.Where(i => i.HasFlag(ReportIssueType.Planned)).ToList();
        AddedAfterSprintStart = Issues.Where(i => i.HasFlag(ReportIssueType.Unplanned)).ToList();
        RemovedAfterSprintStart = RemovedIssues.Where(i => i.HasFlag(ReportIssueType.Planned)).ToList();
        AddedAndRemovedAfterSprintStart = RemovedIssues.Where(i => i.HasFlag(ReportIssueType.Unplanned)).ToList();
        IssueChanges = issueChanges;
        Metrics = new SprintMetrics(sprint, Issues);
    }
    
    public IDictionary<string, UserSprintReportEnriched> GroupByUser()
    {
        var users = Issues
            .Where(i => i.Assignee != null)
            .GroupBy(i => i.Assignee!.Name)
            .ToDictionary(g => g.Key!, g => new UserSprintReportEnriched(g.Select(i => i.Assignee).FirstOrDefault(), this));
        return users;
    }
}