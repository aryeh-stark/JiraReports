using JiraReportsClient.Configurations;
using JiraReportsClient.Http;
using FluentAssertions;
using Serilog;

namespace JiraReportsClientTests.Http;

public partial class JiraHttpClientTests
{

    
    [Fact]
    public async Task TestGetBoardsForProjectAsync()
    {
        var boards = await Client.GetBoardsForProjectAsync("EW");
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
}