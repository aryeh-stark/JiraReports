using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Issues;
using JiraReportsClient.Entities.Reports.SprintBurndowns;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Sprints;
using JiraReportsClient.Http;

namespace JiraReportsClient;

public partial class JiraClient
{
    public async Task<IReadOnlyList<Board>> GetBoardsForProjectAsync(string projectKey)
    {
        var projectJiraBoards = await _client.GetBoardsForProjectAsync(projectKey);
        var projectBoards = projectJiraBoards.ToBoardList();
        return projectBoards;
    }

    public async Task<Board?> GetBoardByIdAsync(int boardId)
    {
        var jiraBoard = await _client.GetBoardByIdAsync(boardId);
        var board = jiraBoard?.ToBoardModel();
        return board;
    }

    public async Task<IReadOnlyList<Board>> GetBoardsAsync()
    {
        var jiraBoards = await _client.GetBoardsAsync();
        var boards = jiraBoards.ToBoardList();
        return boards;
    }

    public async Task<(bool, Board? board)> IsBoardScrum(int boardId)
    {
        var (isScrumBoard, jiraBoard) = await _client.IsBoardScrum(boardId);
        var board = jiraBoard?.ToBoardModel();
        return (isScrumBoard, board);
    }
}