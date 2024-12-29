using System.Diagnostics;
using System.Text.Json.Serialization;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Boards.Atlassian;
using JiraReportsClient.Entities.Sprints.Atlassian;

namespace JiraReportsClient.Entities.Sprints;

[DebuggerDisplay("Sprint: {Name} ({Id})")]
public class Sprint
{
    public int Id { get; set; }
    public SprintState? State { get; set; }
    public string Name { get; set; }
    public string Goal { get; set; }
    public int SprintVersion { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public Board Board { get; set; }

    public static implicit operator Sprint(JiraSprint jiraSprint)
    {
        return jiraSprint.ToModel();
    }

    public int GetDaysRemaining()
    {
        // If the sprint is already complete, return 0.
        if (CompleteDate.HasValue && CompleteDate.Value <= DateTime.Now)
        {
            return 0;
        }

        // If there's no end date specified, assume 0 days remaining.
        if (!EndDate.HasValue)
        {
            return 0;
        }

        // Calculate days left until EndDate (comparing against today's date).
        var daysLeft = (EndDate.Value.Date - DateTime.Today).TotalDays;

        // If the end date is in the past or within today, return 0, otherwise cast to int.
        return daysLeft > 0 ? (int)daysLeft : 0;
    }
}