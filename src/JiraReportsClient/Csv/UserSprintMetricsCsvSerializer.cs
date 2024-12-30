using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;

namespace JiraReportsClient.Csv;

public static class UserSprintMetricsCsvSerializer 
{
    public static string Serialize(IEnumerable<UserSprintReportEnriched> userReports)
    {
        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

        var records = userReports.Select(report => new UserSprintMetricsRecord(
            report.User?.Name ?? "Unassigned",
            report.Metrics
        ));

        csv.WriteRecords(records);
        return writer.ToString();
    }
}