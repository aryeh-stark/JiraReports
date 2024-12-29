using System.Text.Json;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Issues.Atlassian;
using JiraReportsClient.Entities.Reports.SprintReports.Atlassian;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Utils.Json.Converters;
using IssueStatus = JiraReportsClient.Entities.Issues.IssueStatus;
using IssueType = JiraReportsClient.Entities.Issues.IssueType;

namespace JiraReportsClient.Entities.Reports.SprintReports;

public static class SprintReportExtensions
{

    public static string ToJson(this SprintReportEnriched? reportEnriched, bool indented = true)
    {
        if (reportEnriched == null) return string.Empty;
        
        var options = new JsonSerializerOptions
        {
            WriteIndented = indented, // Pretty-print JSON if true
            DefaultIgnoreCondition = JsonIgnoreCondition.Never, // Ignore null values
            ReferenceHandler = ReferenceHandler.IgnoreCycles // Avoid circular references
        };
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new EstimationTimeJsonConverter());
        options.Converters.Add(new FixedDoubleJsonConverter());

        return JsonSerializer.Serialize(reportEnriched, options);
    }
}