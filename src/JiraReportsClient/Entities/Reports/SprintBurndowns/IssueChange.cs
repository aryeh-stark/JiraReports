using System.Diagnostics;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

[DebuggerDisplay("{Timestamp.ToString(\"yyyy/MM/dd HH:mm:ss\")}")]
public class IssueChange
{
    
    public IssueChange(string key, bool isAddedToSprint, bool isAddEvent, bool isBurndownEvent, bool isDone, bool isRemoveEvent, long epochTimestamp, bool? added, double? estimationValue)
    {
        Key = key;
        IsAddedToSprint = isAddedToSprint;
        IsAddEvent = isAddEvent;
        IsBurndownEvent = isBurndownEvent;
        IsDone = isDone;
        IsRemoveEvent = isRemoveEvent;
        EpochTimestamp = epochTimestamp;
        Added = added;
        EstimationValue = estimationValue;
    }

    public string Key { get; set; }
    
    public bool IsDone { get; set; }

    public bool IsAddEvent { get; set; }

    public bool IsRemoveEvent { get; set; }

    public bool IsBurndownEvent { get; set; }

    public bool IsAddedToSprint { get; set; }
    
    public long EpochTimestamp { get; set; }
    
    public DateTime Timestamp => DateTimeOffset.FromUnixTimeMilliseconds(EpochTimestamp).DateTime;
    
    public bool? Added { get; set; }
    
    public double? EstimationValue { get; set; }
}
