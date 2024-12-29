using System.Diagnostics;
using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient.Entities.Sprints;

[DebuggerDisplay("{Key} - {Summary}")]
public class SprintIssue : Issue
{
    public bool Hidden { get; set; }
    public int TypeHierarchyLevel { get; set; }
    public bool Done { get; set; }
    public bool Flagged { get; set; }
    public double? CurrentEstimateStatistic { get; set; }
    public double? EstimateStatistic { get; set; }
    public int ProjectId { get; set; }
    public List<int> SprintIds { get; set; }
    public long UpdatedAt { get; set; }
}