using JiraReportsClient.Entities.Issues;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

public static class ChangeExtensions
{
    public static bool IsAddedToSprint(this Change change)
    {
        return change.Added is true;
    }

    public static bool IsRemovedFromSprint(this Change change) => change.Added is false;

    public static bool IsBurndownEvent(this Change change)
    {
        return change.Added.HasValue == false
               && change.Column != null
               && change.Column.NotDone.HasValue
               && change.Column.NotDone.Value == false
               && change.Column.Done.GetValueOrDefault(false)
               && change.Column.NewStatus.GetValueOrDefault(-1) > 0;
    }
    
    public static IssueChangeType GetIssueChangeType(this Change change)
    {
        if (change.IsAddedToSprint() && !change.IsRemovedFromSprint() && !change.IsBurndownEvent()) 
        {
            return IssueChangeType.AddedToSprint;
        }

        if (change.IsRemovedFromSprint() && !change.IsAddedToSprint() && !change.IsBurndownEvent())
        {
            return IssueChangeType.RemovedFromSprint;
        }

        if (change.IsBurndownEvent() && !change.IsAddedToSprint() && !change.IsRemovedFromSprint())
        {
            return IssueChangeType.BurndownEvent;
        }

        return IssueChangeType.Undefined;
    }
}