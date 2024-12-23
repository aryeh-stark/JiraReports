namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public static class BurndownProcessor
{
    public static Dictionary<string, List<IssueChange>> RestructureBurndownData(Dictionary<long, List<Change>> changes)
    {
        var result = new Dictionary<string, List<IssueChange>>();

        foreach (var timestampEntry in changes)
        {
            foreach (var change in timestampEntry.Value)
            {
                if (!result.ContainsKey( change.IssueKey))
                {
                    result[change.IssueKey] = [];
                }

                var issueChange = new IssueChange(
                    isAddedToSprint: change.IsAddedToSprint(),
                    isAddEvent: change.IsAddEvent(), 
                    isBurndownEvent: change.IsBurndownEvent(), 
                    isDone: change.IsDone(),
                    isRemoveEvent: change.IsRemoveEvent(), 
                    epochTimestamp:  timestampEntry.Key, 
                    added: change.Added,
                    estimationValue: change.StatC?.NewValue, 
                    key: change.IssueKey);

                result[change.IssueKey].Add(issueChange);
            }
        }

        // Sort changes by timestamp for each issue
        foreach (var value in result.Values)
        {
            value.Sort((a, b) => a.EpochTimestamp.CompareTo(b.EpochTimestamp));
        }

        return result;
    }
}