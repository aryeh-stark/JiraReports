using JiraReportsClient.Entities.Boards.Atlassian;
using JiraReportsClient.Entities.Projects;

namespace JiraReportsClient.Entities.Boards;

public static class BoardExtensions
{
    public static Project ToProjectModel(this JiraBoardLocation boardLocation)
    {
        return new Project
        {
            Id = boardLocation.ProjectId.GetValueOrDefault(-1),
            Key = boardLocation.ProjectKey,
            Name = boardLocation.ProjectName,
        };
    }

    public static Board ToBoardModel(this JiraBoard jiraBoard)
    {
        return new Board
        {
            Id = jiraBoard.Id,
            Name = jiraBoard.Name,
            Type = jiraBoard.Type,
            IsPrivate = jiraBoard.IsPrivate,
            Project = jiraBoard.Location.ToProjectModel(),
        };
    }
    
    public static IReadOnlyList<Board> ToBoardList(this IEnumerable<JiraBoard> jiraBoards)
    {
        return jiraBoards.Select(b => b.ToBoardModel()).ToList();
    }
}