using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient.Entities.Reports.SprintReports
{
    public class SprintReportModel
    {
        public Sprint Sprint { get; set; }

        public List<Issue> CompletedIssues { get; set; }

        public List<Issue> IssuesNotCompleted { get; set; }

        public List<Issue> PuntedIssues { get; set; }

        public List<Issue> IssuesCompletedInAnotherSprint { get; set; } 

        public Dictionary<string, bool> IssueKeysAddedDuringSprint { get; set; } 

        public SprintEstimateStatistics EstimateStatistics { get; set; }

        public List<Issue> GetAllIssues()
        {
            return CompletedIssues
                .Concat(IssuesNotCompleted)
                .Concat(PuntedIssues)
                .Concat(IssuesCompletedInAnotherSprint)
                .DistinctBy(i => i.Key)  // .NET 6 DistinctBy
                .OrderBy(i => i.Key)
                .ToList();
        }

        public List<string> GetAllIssueKeys()
        {
            return GetAllIssues()
                .Select(i => i.Key)
                .ToList();
        }
    }
}