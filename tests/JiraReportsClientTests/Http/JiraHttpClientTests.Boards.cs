using JiraReportsClient.Configurations;
using JiraReportsClient.Http;
using FluentAssertions;
using Serilog;
using JiraReportsClient.Entities.Boards;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{
    [Fact]
    public async Task TestGetBoardsForProjectAsync()
    {
        var jiraBoards = await Client.GetBoardsForProjectAsync("EW");
        jiraBoards.Should().NotBeEmpty();
        
        var boards = jiraBoards.ToBoardList();
        boards.Should().NotBeEmpty();
        
    }
    
    [Fact]
    public async Task TestGetBoardByIdAsync()
    {
        var boards = await Client.GetBoardsForProjectAsync("EW");
        boards.Should().NotBeEmpty();
        
        var boardId = boards.First().Id;
        var board = await Client.GetBoardByIdAsync(boardId);
        board.Should().NotBeNull();
    }

    [Fact]
    public async Task TestGetBoardsAsync()
    {
        var boards = await Client.GetBoardsAsync();
        boards.Should().NotBeEmpty();
    }

    [Fact]
    public async Task IsBoardScrumTest()
    {
        var boards = await Client.GetBoardsAsync();
        boards.Should().NotBeEmpty();
        
        var scrumBoard = boards.FirstOrDefault(b => b.Type == BoardTypes.Scrum);
        scrumBoard.Should().NotBeNull();

        var (isScrumBoard, board) = await Client.IsBoardScrum(scrumBoard!.Id);
        isScrumBoard.Should().BeTrue();
        board.Should().NotBeNull();
    }
}