using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JiraCmdLineTool.Common.Objects.SprintReports;

public class SprintDetail
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sequence")]
    public int Sequence { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("linkedPagesCount")]
    public int LinkedPagesCount { get; set; }

    [JsonPropertyName("goal")]
    public string Goal { get; set; }

    [JsonPropertyName("sprintVersion")]
    public int SprintVersion { get; set; }

    [JsonPropertyName("startDate")]
    public string StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public string EndDate { get; set; }

    [JsonPropertyName("completeDate")]
    public string CompleteDate { get; set; }

    [JsonPropertyName("isoStartDate")]
    public string IsoStartDate { get; set; }

    [JsonPropertyName("isoEndDate")]
    public string IsoEndDate { get; set; }

    [JsonPropertyName("isoCompleteDate")]
    public string IsoCompleteDate { get; set; }

    [JsonPropertyName("canUpdateSprint")]
    public bool CanUpdateSprint { get; set; }

    [JsonPropertyName("remoteLinks")]
    public List<object> RemoteLinks { get; set; }

    [JsonPropertyName("daysRemaining")]
    public int DaysRemaining { get; set; }
}