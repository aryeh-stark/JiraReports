namespace JiraReportsClient.Entities.Projects;

public static class ProjectExtensions
{
    public static IReadOnlyList<Project> ToProjectsList(this IEnumerable<Project> jiraProjects)
    {
        return jiraProjects.ToList();
    }
}