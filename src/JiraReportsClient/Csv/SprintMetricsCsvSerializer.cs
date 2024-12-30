using System.Text;
using JiraReportsClient.Entities.Reports.SprintReports;
using CsvHelper;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
using JiraReportsClient.Entities.Reports.SprintReports.Metrics;

namespace JiraReportsClient.Csv;

public static class SprintMetricsCsvSerializer 
{
    public static string Serialize(params SprintReportEnriched[] sprintReports)
    {
        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        });

        var records = sprintReports
            .Select(report => new SprintMetricsRecord(report.Metrics))
            .ToList();

        csv.WriteRecords(records);
        return writer.ToString();
    }
}