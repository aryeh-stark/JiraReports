using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Projects;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient.Entities.Issues;

public class IssueFields
    {
        [JsonPropertyName("statuscategorychangedate")]
        public string StatusCategoryChangeDate { get; set; }

        [JsonPropertyName("parent")]
        public ParentIssue Parent { get; set; }

        [JsonPropertyName("priority")]
        public Priority Priority { get; set; }

        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; }

        [JsonPropertyName("timeestimate")]
        public int? TimeEstimate { get; set; }

        [JsonPropertyName("aggregatetimeoriginalestimate")]
        public int? AggregateTimeOriginalEstimate { get; set; }

        [JsonPropertyName("assignee")]
        public User Assignee { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("aggregatetimeestimate")]
        public int? AggregateTimeEstimate { get; set; }

        [JsonPropertyName("creator")]
        public User Creator { get; set; }

        [JsonPropertyName("reporter")]
        public User Reporter { get; set; }

        [JsonPropertyName("issuetype")]
        public IssueType IssueType { get; set; }

        [JsonPropertyName("project")]
        public Project Project { get; set; }

        [JsonPropertyName("customfield_10020")]
        public List<Sprint> RelatedSprints { get; set; }

        [JsonPropertyName("updated")]
        public string Updated { get; set; }

        [JsonPropertyName("timeoriginalestimate")]
        public int? TimeOriginalEstimate { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("customfield_10001")]
        public Team Team { get; set; }

        [JsonPropertyName("duedate")]
        public string DueDate { get; set; }
    }
