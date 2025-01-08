using System.Collections.Concurrent;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient;

public partial class JiraClient
{
    public async Task<IReadOnlyDictionary<int, IEnumerable<SprintReportEnriched>>>
        GetAllClosedSprintReportsForBoardNamesAsync(
            params string[] boardNames)
    {
        var boards = await GetBoardsAsync();
        var scrumBoards = boards
            .Where(b => b.Type == BoardTypes.Scrum)
            .Where(b => boardNames.Any(b.IsEqual))
            .ToList();

        var sprintReportsByBoardId = new Dictionary<int, IEnumerable<SprintReportEnriched>>();
        foreach (var board in scrumBoards)
        {
            var sprintReports = await GetSprintReportsByBoard(board);
            if (sprintReports == null) continue;
            sprintReportsByBoardId[board.Id] = sprintReports;
        }

        return sprintReportsByBoardId;

        async Task<IEnumerable<SprintReportEnriched>?> GetSprintReportsByBoard(Board board)
        {
            var sprints = await GetSprintsForBoardIdAsync(board, includeClosedSprints: true);
            var sprintReports = new List<SprintReportEnriched>();
            foreach (var sprint in sprints.Where(s => s.State == SprintState.Closed && s.IsValidSequenceId()))
            {
                var sprintReport = await GetSprintReportEnrichedAsync(board, sprint.Id);
                if (sprintReport == null) continue;
                sprintReports.Add(sprintReport);
            }

            return sprintReports;
        }
    }

    public async Task<IEnumerable<SprintReportEnriched>> GetAllClosedSprintReportsForBoardNameAsync(string boardName)
    {
        var boards = await GetBoardsAsync();
        var board = boards
            .Where(b => b.Type == BoardTypes.Scrum)
            .FirstOrDefault(b => b.IsEqual(boardName));

        var sprintReports = new List<SprintReportEnriched>();
        if (board == null) return sprintReports;

        var unverifiedSprints = await GetSprintsForBoardIdAsync(board, includeClosedSprints: true);
        var sprints = unverifiedSprints.Where(s => s.IsValidSequenceId()).ToList();
        foreach (var sprint in sprints.Where(s => s.IsValidSequenceId()))
        {
            var sprintReport = await GetSprintReportEnrichedAsync(board, sprint.Id);
            if (sprintReport == null) continue;
            sprintReports.Add(sprintReport);
        }

        return sprintReports;
    }

    public async Task<IReadOnlyDictionary<int, SprintReportEnriched>> GetLastClosedSprintReportsForBoardNamesAsync(
            params string[] boardNames)
    {
        var boards = await GetBoardsAsync();
        var scrumBoards = boards
            .Where(b => b.Type == BoardTypes.Scrum)
            .Where(b => boardNames.Any(b.IsEqual))
            .ToList();

        var sprintReportsByBoardId = new Dictionary<int, SprintReportEnriched>();
        foreach (var board in scrumBoards)
        {
            var sprintReports = await GetLastSprintReportsByBoard(board);
            if (sprintReports == null) continue;
            sprintReportsByBoardId[board.Id] = sprintReports;
        }

        return sprintReportsByBoardId;

        async Task<SprintReportEnriched?> GetLastSprintReportsByBoard(Board board)
        {
            var sprints = await GetSprintsForBoardIdAsync(board, includeClosedSprints: true);
            var lastClosedSprint = sprints
                .Where(s => s.IsValidSequenceId())
                .Where(s => s.State == SprintState.Closed)
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();
            
            if (lastClosedSprint == null) return null;
            
            var sprintReport = await GetSprintReportEnrichedAsync(board, lastClosedSprint.Id);
            return sprintReport;
        }
    }
}