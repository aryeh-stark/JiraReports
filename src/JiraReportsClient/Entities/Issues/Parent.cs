using System.Diagnostics;
using System.Text.Json.Serialization;

namespace JiraReportsClient.Entities.Issues;

[DebuggerDisplay("[{Key}] [{Type}] {Summary}")]
public class Parent
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Key { get; set; }
    public string Summary { get; set; }
    public IssueStatus Status { get; set; }
    public IssuePriority Priority { get; set; }
    public IssueType Type { get; set; }
}