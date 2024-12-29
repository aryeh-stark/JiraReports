using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Utils.Json;
using JiraReportsClient.Utils.Json.Converters;

namespace JiraReportsClient.Entities.Reports.SprintReports.Atlassian;

[DebuggerDisplay("{Key} - {Summary}")]
public class SprintIssueDetails
{
    [JsonPropertyName("id")]
    [JsonConverter(typeof(CustomStringToIntConverter))]
    public int Id { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("hidden")]
    public bool Hidden { get; set; }

    [JsonPropertyName("parentId")]
    [JsonConverter(typeof(CustomStringToIntConverter))]
    public int ParentId { get; set; }

    [JsonPropertyName("parentKey")]
    public string ParentKey { get; set; }

    [JsonPropertyName("typeName")]
    public string TypeName { get; set; }

    [JsonPropertyName("typeId")]
    public string TypeId { get; set; }

    [JsonPropertyName("typeHierarchyLevel")]
    public int TypeHierarchyLevel { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    [JsonPropertyName("typeUrl")]
    public string TypeUrl { get; set; }

    [JsonPropertyName("priorityUrl")]
    public string PriorityUrl { get; set; }

    [JsonPropertyName("priorityName")]
    public string PriorityName { get; set; }

    [JsonPropertyName("priorityId")]
    public string PriorityId { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }

    [JsonPropertyName("assignee")]
    public string Assignee { get; set; }

    [JsonPropertyName("assigneeKey")]
    public string AssigneeKey { get; set; }

    [JsonPropertyName("assigneeAccountId")]
    public string AssigneeAccountId { get; set; }

    [JsonPropertyName("assigneeName")]
    public string AssigneeName { get; set; }

    [JsonPropertyName("avatarUrl")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("hasCustomUserAvatar")]
    public bool HasCustomUserAvatar { get; set; }

    [JsonPropertyName("flagged")]
    public bool Flagged { get; set; }

    [JsonPropertyName("labels")]
    public List<string> Labels { get; set; }

    [JsonPropertyName("epic")]
    public string Epic { get; set; }

    [JsonPropertyName("epicField")]
    public JiraEpic JiraEpic { get; set; }

    [JsonPropertyName("currentEstimateStatistic")]
    public EstimateStatistic CurrentEstimateStatistic { get; set; }

    [JsonPropertyName("estimateStatisticRequired")]
    public bool EstimateStatisticRequired { get; set; }

    [JsonPropertyName("estimateStatistic")]
    public EstimateStatistic EstimateStatistic { get; set; }

    [JsonPropertyName("statusId")]
    public string StatusId { get; set; }

    [JsonPropertyName("statusName")]
    public string StatusName { get; set; }

    [JsonPropertyName("statusUrl")]
    public string StatusUrl { get; set; }

    [JsonPropertyName("status")]
    public IssueStatus Status { get; set; }

    [JsonPropertyName("fixVersions")]
    public List<object> FixVersions { get; set; }

    [JsonPropertyName("projectId")]
    public int ProjectId { get; set; }

    [JsonPropertyName("linkedPagesCount")]
    public int LinkedPagesCount { get; set; }

    [JsonPropertyName("sprintIds")]
    public List<int> SprintIds { get; set; }

    [JsonPropertyName("updatedAt")]
    public long UpdatedAt { get; set; }
}