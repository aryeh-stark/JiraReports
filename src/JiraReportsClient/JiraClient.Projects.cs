using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Projects;
using JiraReportsClient.Entities.Reports.SprintBurndowns;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Http;

namespace JiraReportsClient;

public partial class JiraClient
{
    public async Task<IReadOnlyList<Project>> GetProjectsAsync()
    {
        var jiraProjects = await client.GetProjectsAsync();
        var projects = jiraProjects.ToProjectsList();
        return projects;
    }
}