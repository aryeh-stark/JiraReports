using System.Diagnostics;
using JiraReportsClient.Entities.Boards.Atlassian;
using JiraReportsClient.Entities.Projects;

namespace JiraReportsClient.Entities.Boards;

[DebuggerDisplay("Board: {Name} (Id: {Id}, Type: {Type})")]
public class Board
{
    public int Id { get; set; }
    public string Name { get; set; }
    public BoardTypes Type { get; set; } 
    public bool IsPrivate { get; set; }
    public Project Project { get; set; }
    
    public static implicit operator Board(JiraBoard jiraBoard)
    {
        return jiraBoard.ToBoardModel();
    }
}