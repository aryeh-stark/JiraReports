using FluentAssertions;
using JiraReportsClient.Entities.Boards;

namespace JiraReportsClientTests;

public partial class JiraClientTests
{
    [Fact]
    public async Task GetBoardsForProjectAsyncTest()
    {
        var boards = await Client.GetBoardsForProjectAsync("EW");
        boards.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetBoardByIdAsyncTest()
    {
        var board = await Client.GetBoardByIdAsync(84);
        board.Should().NotBeNull();
    }

    // [Fact]
    // public async Task GetBoardsAsyncTest()
    // {
    //     var boards = await Client.GetBoardsAsync();
    //     boards.Should().NotBeEmpty();
    // }

    [Fact]
    public async Task IsBoardScrumTest()
    {
        var board = await Client.GetBoardByIdAsync(84);
        board.Should().NotBeNull();
        board!.Type.Should().Be(BoardTypes.Scrum);
    }
}