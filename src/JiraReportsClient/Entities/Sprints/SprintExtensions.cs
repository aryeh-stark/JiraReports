using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Sprints.Atlassian;

namespace JiraReportsClient.Entities.Sprints;

public static class SprintExtensions
{
    public static SprintState ToModel(this string state)
    {
        return state switch
        {
            "future" => SprintState.Future,
            "active" => SprintState.Active,
            "closed" => SprintState.Closed,
            _ => SprintState.Undefined
        };
    }
    
    public static Sprint ToModel(this JiraSprint jiraSprint)
    {
        return new Sprint
        {
            Id = jiraSprint.Id,
            Name = jiraSprint.Name,
            StartDate = jiraSprint.StartDate,
            EndDate = jiraSprint.EndDate,
            CompleteDate = jiraSprint.CompleteDate,
            CreatedDate = jiraSprint.CreatedDate,
            Goal = jiraSprint.Goal,
            State = jiraSprint.State.ToModel(),
            Board = jiraSprint.JiraBoard.ToBoardModel()
        };
    }
    
    public static List<Sprint> ToSprintsList(this IEnumerable<JiraSprint> jiraSprints)
    {
        return jiraSprints.Select(s => s.ToModel()).ToList();
    }
}