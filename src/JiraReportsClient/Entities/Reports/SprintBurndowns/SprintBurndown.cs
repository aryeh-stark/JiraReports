using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public class SprintBurndown
{
    [JsonPropertyName("changes")]
    public Dictionary<long, List<Change>> Changes { get; set; }

    [JsonPropertyName("startTime")]
    public long StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public long EndTime { get; set; }

    [JsonPropertyName("completeTime")]
    public long CompleteTime { get; set; }

    [JsonPropertyName("now")]
    public long Now { get; set; }

    [JsonPropertyName("statisticField")]
    public StatisticField StatisticField { get; set; }

    [JsonPropertyName("issueToParentKeys")]
    public Dictionary<string, string> IssueToParentKeys { get; set; }

    [JsonPropertyName("issueToSummary")]
    public Dictionary<string, string> IssueToSummary { get; set; }

    [JsonPropertyName("workRateData")]
    public WorkRateData WorkRateData { get; set; }
}
