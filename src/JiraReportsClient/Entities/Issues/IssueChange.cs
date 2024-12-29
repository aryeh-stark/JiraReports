using System.Diagnostics;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("{Key} {ChangeType} - {ChangeTime.ToString(\"yyyy/MM/dd HH:mm\")}")]
public class IssueChange(
    long sprintStartEpochTimestamp,
    long changeEpochTimestamp,
    string key,
    IssueChangeType changeType)
{
    public long ChangeEpochTime { get; init; } = changeEpochTimestamp;
    public long SprintStartEpochTime { get; init; } = sprintStartEpochTimestamp;
    public DateTime ChangeTime { get; init; } = DateTimeOffset.FromUnixTimeMilliseconds(changeEpochTimestamp).DateTime;

    public DateTime SprintStartTime { get; init; } =
        DateTimeOffset.FromUnixTimeMilliseconds(sprintStartEpochTimestamp).DateTime;

    public string Key { get; } = key;

    public IssueChangeType ChangeType { get; } = changeType;

    public bool IsAddedToSprint() => ChangeType == IssueChangeType.AddedToSprint;
    public bool IsRemovedFromSprint() => ChangeType == IssueChangeType.RemovedFromSprint;
    public bool IsBurndownEvent() => ChangeType == IssueChangeType.BurndownEvent;
}