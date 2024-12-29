using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Reports.SprintBurndowns;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Http;

namespace JiraReportsClient;

public partial class JiraClient(JiraHttpClient client)
{
    private readonly JiraHttpClient _client = client;
    
}