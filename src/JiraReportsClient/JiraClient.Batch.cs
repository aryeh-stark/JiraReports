using System.Collections.Concurrent;
using JiraReportsClient.Entities.Boards;
using JiraReportsClient.Entities.Reports.SprintReports;
using JiraReportsClient.Entities.Sprints;

namespace JiraReportsClient;

public partial class JiraClient
{
    public async Task<IReadOnlyDictionary<BoardRecord, IEnumerable<SprintReportEnriched>>>
        GetAllClosedSprintReportsForBoardNamesAsync(
            int last,
            params string[] boardNames)
    {
        var boards = await GetBoardsAsync();
        var scrumBoards = boards
            .Where(b => b.Type == BoardTypes.Scrum)
            .Where(b => boardNames.Any(bn => bn.Contains(b.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var sprintReportsByBoardId = new Dictionary<BoardRecord, IEnumerable<SprintReportEnriched>>();
        foreach (var board in scrumBoards)
        {
            var sprintReports = await GetSprintReportsByBoard(board);
            if (sprintReports == null) continue;
            sprintReportsByBoardId[board] = sprintReports;
        }

        return sprintReportsByBoardId;

        async Task<IEnumerable<SprintReportEnriched>?> GetSprintReportsByBoard(Board board)
        {
            var sprints = await GetSprintsForBoardIdAsync(board, includeClosedSprints: true);
            var sprintReports = new List<SprintReportEnriched>();
            var sprintsToQuery = sprints
                .Where(s => s.State == SprintState.Closed)
                .Where(s => s.Name.Contains(board.Name, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.EndDate)
                .Take(last)
                .ToList();
            foreach (var sprint in sprintsToQuery)
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

        var sprints = await GetSprintsForBoardIdAsync(board, includeClosedSprints: true);
        var sprintsToQuery = sprints
            .Where(s => s.State == SprintState.Closed)
            .Where(s => s.Name.Contains(board.Name, StringComparison.OrdinalIgnoreCase))
            .Where(s => s.IsValidSequenceId())
            .OrderByDescending(s => s.EndDate)
            .ToList();
        foreach (var sprint in sprintsToQuery)
        {
            var sprintReport = await GetSprintReportEnrichedAsync(board, sprint.Id);
            if (sprintReport == null) continue;
            sprintReports.Add(sprintReport);
        }

        return sprintReports;
    }

    public async Task<IReadOnlyDictionary<BoardRecord, SprintReportEnriched>> GetLastClosedSprintReportsForBoardNamesAsync(params string[] boardNames)
    {
        var boards = await GetBoardsAsync();
        var scrumBoards = boards
            .Where(b => b.Type == BoardTypes.Scrum)
            .Where(b => boardNames.Any(b.IsEqual))
            .ToList();

        var sprintReportsByBoardId = new Dictionary<BoardRecord, SprintReportEnriched>();
        foreach (var board in scrumBoards)
        {
            var sprintReports = await GetLastSprintReportsByBoard(board);
            if (sprintReports == null) continue;
            sprintReportsByBoardId[board] = sprintReports;
        }

        return sprintReportsByBoardId;

        async Task<SprintReportEnriched?> GetLastSprintReportsByBoard(Board board)
        {
            var sprints = await GetSprintsForBoardIdAsync(board, includeClosedSprints: true);
            var lastClosedSprint = sprints
                .Where(s => s.IsValidSequenceId())
                .Where(s => s.State == SprintState.Closed)
                .Where(s => s.Name.Contains(board.Name, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();
            
            if (lastClosedSprint == null) return null;
            
            var sprintReport = await GetSprintReportEnrichedAsync(board, lastClosedSprint.Id);
            return sprintReport;
        }
    }
}