using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Reports.SprintBurndowns;

[DebuggerDisplay("{IssueKey}")]
public class Change
{
    [JsonPropertyName("key")]
    public string IssueKey { get; set; }

    [JsonPropertyName("statC")]
    public StatC? StatC { get; set; }

    [JsonPropertyName("column")]
    public Column? Column { get; set; }

    [JsonPropertyName("added")]
    public bool? Added { get; set; }
    
    
    
    public bool IsDone() => Column?.Done == true;

    public bool IsAddEvent() => Added is true;

    public bool IsRemoveEvent()
    {
        return Added is false && Column == null && StatC == null;
    }


}
