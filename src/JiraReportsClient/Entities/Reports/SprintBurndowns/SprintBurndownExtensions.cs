using System.Collections.ObjectModel;
using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public static class SprintBurndownExtensions
{
    private static IReadOnlyList<T> AsReadOnly<T>(this IEnumerable<T> enumerable) => (IReadOnlyList<T>)enumerable.ToList();
    
    public static IReadOnlyDictionary<string, IReadOnlyList<IssueChange>> ExtractIssueChanges(this SprintBurndown sprintBurndown)
    {
        var result = new Dictionary<string, List<IssueChange>>();

        foreach (var timestampEntry in sprintBurndown.Changes.Where(x => x.Key >= sprintBurndown.StartTime))
        {
            foreach (var change in timestampEntry.Value.Where(change => change.GetIssueChangeType() != IssueChangeType.Undefined))
            {
                if (!result.TryGetValue(change.IssueKey, out var value))
                {
                    value = [];
                    result[change.IssueKey] = value;
                } 

                var issueChange = new IssueChange(sprintBurndown.StartTime, timestampEntry.Key, change.IssueKey, change.GetIssueChangeType());
                value.Add(issueChange);
            }
        }
        var orderedResult = result.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.OrderBy(x => x.ChangeEpochTime).AsReadOnly());
        return new ReadOnlyDictionary<string, IReadOnlyList<IssueChange>>(orderedResult);
    }
}